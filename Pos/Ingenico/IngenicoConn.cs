using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos.Ingenico
{
    public class IngenicoConn
    {
        static public UInt32 CurrentInterface;
        static public UInt64 ActiveTransactionHandle;

        static public Dictionary<UInt32, byte[]> TransactionUniqueIdList = new Dictionary<uint, byte[]>();
        static public Dictionary<UInt32, ST_DEPARTMENT[]> TransactionDepartmentList = new Dictionary<uint, ST_DEPARTMENT[]>();
        static public Dictionary<UInt32, ST_TAX_RATE[]> TransactionTaxRateList = new Dictionary<uint, ST_TAX_RATE[]>();
        static public Dictionary<UInt32, ST_EXCHANGE[]> TransactionExchangeList = new Dictionary<uint, ST_EXCHANGE[]>();
        public static UInt64 ACTIVE_TRX_HANDLE
        {
            get { return ActiveTransactionHandle; }
            set
            {
                ActiveTransactionHandle = value;
                // m_lblCurrentTransactionHandle.Text = value.ToString("X2");
                //lbl_AktifHandle.Text = value.ToString("X2");
                string lbl_AktifHandle = value.ToString("X2");
            }
        }

        public static TreeView m_tvEcho = new TreeView();
        ParserClass parsClass;
        // DisplayClass dispClass;
        ErrorClass errClass;

        bool ACTIVATE_PING = false;

        int numberOfTotalDepartments;
        int numberOfTotalTaxRates;
        int numberOfTotalRecordsReceived = 0;

        public static ST_DEPARTMENT[] stDepartments;
        public static ST_TAX_RATE[] stTaxRates;
        public static ST_GMP_PAIR_RESP pairingResp;

        public static byte[] m_dllVersion = new byte[24];

        byte[] TsmSign = null;

        public bool BatchMode = false;

        EPaymentTypes m_PaymentType = 0;

        List<ST_ITEM> stItemList = new List<ST_ITEM>();

        public static ListView m_listPayment = new ListView();

        uint[] userMessageFlags = new uint[40];


        byte[] m_FileDirBitmap = new byte[128];


        string AnlikTarih;


        public byte isBackground = 0;


        string lbl_Tarih_In = DateTime.Now.ToShortDateString();
        string lbl_Zaman = DateTime.Now.ToLongTimeString();
        string lblHata = "";

        public static ST_ECHO stEcho = new ST_ECHO();
        public string EslestirmeYap(bool manuel = false)
        {
            ParserClass.MainControls = this;

            parsClass = new ParserClass();
            //          dispClass = new DisplayClass();
            errClass = new ErrorClass();


            //           DisplayClass.dispCls = this;
            ParserClass.MainControls = new IngenicoConn();
            ErrorClass.errCls = new IngenicoConn();
            //          Test.TestForm = this;




            Array.Clear(m_dllVersion, 0, m_dllVersion.Length);

            UInt32 ret = GMPSmartDLL.GMP_GetDllVersion(m_dllVersion);
            if (ret != 0)
            {
                MessageBox.Show("Uncompatible DLL Version.\nDll Version Expected (minimum): " + Defines.DLL_VERSION_MIN + "\nDll Version Found : " + GMP_Tools.SetEncoding(m_dllVersion), "ERROR", MessageBoxButtons.OK);
            }
            else if (String.Compare(GMP_Tools.SetEncoding(m_dllVersion), Defines.DLL_VERSION_MIN) < 0)
            {
                MessageBox.Show("Uncompatible DLL Version.\nDll Version Expected (minimum): " + Defines.DLL_VERSION_MIN + "\nDll Version Found : " + GMP_Tools.SetEncoding(m_dllVersion), "ERROR", MessageBoxButtons.OK);
            }

            UInt32[] InterfaceList = new UInt32[20];
            
            UInt32 InterfaceCount = GMPSmartDLL.FP3_GetInterfaceHandleList(InterfaceList, (UInt32)InterfaceList.Length);

            for (UInt32 Index = 0; Index < InterfaceCount; ++Index)
            {
                byte[] ID = new byte[64];
                GMPSmartDLL.FP3_GetInterfaceID(InterfaceList[Index], ID, (UInt32)ID.Length);
                string Handle = InterfaceList[Index].ToString("X8") + "-" + GMP_Tools.SetEncoding(ID);

                TreeNode HandleTree = new TreeNode(Handle);
                HandleTree.Tag = InterfaceList[Index];
                m_treeHandleList.Nodes.Add(HandleTree);



            }

            m_treeHandleList.SelectedNode = m_treeHandleList.Nodes[0];

            CurrentInterface = (uint)m_treeHandleList.Nodes[0].Tag;


            string sonuc = "";
            //listBox1.Items.Add(DateTime.Now.ToString());
            AnlikTarih = DateTime.Now.ToString();
            
            ST_GMP_PAIR pairing = new ST_GMP_PAIR();
            UInt64 TranHandle = 0;
            UInt32 RetCode;

            //lblHata.Text = "";

            lblHata = "";

            RetCode = Json_GMPSmartDLL.FP3_Echo(CurrentInterface, ref stEcho, Defines.TIMEOUT_ECHO);

            if (RetCode != Defines.TRAN_RESULT_OK)
            {
                HandleErrorCode(RetCode);
                sonuc = RetCode.ToString();
                return sonuc;
            }

            pairing.szExternalDeviceBrand = "INGENICO";
            pairing.szExternalDeviceModel = "IWE280";
            pairing.szExternalDeviceSerialNumber = "12344567";
            pairing.szEcrSerialNumber = "JHWE20000079";
            pairing.szProcOrderNumber = "000001";
            pairing.szProcDate = lbl_Tarih_In.Substring(0, 2) + lbl_Tarih_In.Substring(3, 2) + lbl_Tarih_In.Substring(6, 2);
            pairing.szProcTime = lbl_Zaman.Substring(0, 2) + lbl_Zaman.Substring(3, 2) + lbl_Zaman.Substring(6, 2);

            RetCode = Json_GMPSmartDLL.FP3_StartPairingInit(CurrentInterface, ref pairing, ref pairingResp);

            if (RetCode != Defines.TRAN_RESULT_OK)
            {
                HandleErrorCode(RetCode);
                sonuc = RetCode.ToString();
                return sonuc;
            }


            ParserClass.DisplayEcrStatus(pairingResp, stEcho);

            // Departman kodları ve kurların alımı
            RetCode = GetDepartments();
            if (RetCode != Defines.TRAN_RESULT_OK)
            {
                HandleErrorCode(RetCode);
                sonuc = RetCode.ToString();
                return sonuc;
            }

            RetCode = GetCurrency();
            if (RetCode != Defines.TRAN_RESULT_OK)
            {
                HandleErrorCode(RetCode);
                sonuc = RetCode.ToString();
                return sonuc;
            }

            // ÖKC'ye bir bağlantı yapıp, içinde tamamlanmamış işlem var mı diye bakılır
            // ÖKC'de gereksiz Unique ID oluşmaması için Unique ID aşağıdaki gibi verilmeli

            byte[] UniqueId = new byte[24] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

            RetCode = GMPSmartDLL.FP3_Start(CurrentInterface, ref TranHandle, isBackground, UniqueId, UniqueId.Length, null, 0, null, 0, Defines.TIMEOUT_DEFAULT);
            AddTrxHandles(CurrentInterface, TranHandle, isBackground);

            int flag = 0;
            if (RetCode == Defines.APP_ERR_ALREADY_DONE)
            {
                if (manuel)
                {
                    DialogResult dr = MessageBox.Show(res_man.GetString("ÖKC'de Tamamlanmamis Islem Var. Islemi IPTAL Etmek için CANCEL, Tekrar Yüklemek için OK 'e basin"), res_man.GetString("ÖKC'de Tamamlanmamis Islem Var. Islemi IPTAL Etmek için CANCEL, Tekrar Yüklemek için OK 'e basin"), MessageBoxButtons.OKCancel);
                    switch (dr)
                    {
                        case DialogResult.OK:
                            RetCode = ReloadTransaction();
                            flag = 1;
                            break;
                        case DialogResult.Cancel:
                            OnBnClickedButtonVoidAll();
                            break;
                    }
                }
                else
                {
                    OnBnClickedButtonVoidAll();
                }
            }

            if (flag != 1)
            {
                UInt64 TransHandle = GetTransactionHandle(CurrentInterface);
                RetCode = Defines.TRAN_RESULT_OK;
                if (TransHandle != 0)
                {
                    RetCode = GMPSmartDLL.FP3_Close(CurrentInterface, TransHandle, Defines.TIMEOUT_DEFAULT);
                    if (RetCode == Defines.TRAN_RESULT_OK)
                        DeleteTrxHandles(CurrentInterface, TransHandle);

                    ClearTransactionUniqueId(CurrentInterface);
                }

                flag = 0;
            }

            HandleErrorCode(RetCode);

            listBox1.Items.Add(DateTime.Now.ToString());

            return sonuc;
        }
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        public void AddIntoCommandBatch(string commandName, int commandType, byte[] buffer, int bufferLen)
        {
            byte[] dataPtr = new byte[bufferLen + 6];
            byte[] type = new byte[4];
            int typeLen = 0;
            StringToByteArray(commandType.ToString("X2"), type, ref typeLen);
            Buffer.BlockCopy(type, 0, dataPtr, 0, 4);

            byte[] bufLen = new byte[2];
            int bufLenLen = 0;

            String TempLen;
            TempLen = bufferLen.ToString("X2");
            if ((TempLen.Length % 2) == 1)
                TempLen = "0" + TempLen;
            StringToByteArray(TempLen, bufLen, ref bufLenLen);
            Buffer.BlockCopy(bufLen, 0, dataPtr, 4, 2);

            Buffer.BlockCopy(buffer, 0, dataPtr, 6, bufferLen);

            ListViewItem item1 = new ListViewItem((m_listBatchCommand.Items.Count + 1).ToString());
            item1.SubItems.Add(commandName);
            item1.SubItems.Add(ByteArrayToString(dataPtr, bufferLen + 6));

            m_listBatchCommand.Items.Add(item1);
        }


        ListBox m_listBatchCommand;

         UInt32 OnBnClickedButtonVoidAll()
        {
            UInt32 RetCode = 0;
            ST_TICKET m_stTicket = new ST_TICKET();

            if (BatchMode)
            {
                byte[] buffer = new byte[1024];
                int bufferLen = 0;

                bufferLen = GMPSmartDLL.prepare_VoidAll(buffer, buffer.Length);
                AddIntoCommandBatch("prepare_VoidAll", Defines.GMP3_FISCAL_PRINTER_MODE_REQ, buffer, bufferLen);

                Array.Clear(buffer, 0, buffer.Length);
                bufferLen = 0;

                bufferLen = GMPSmartDLL.prepare_Close(buffer, buffer.Length);
                AddIntoCommandBatch("prepare_Close", Defines.GMP3_FISCAL_PRINTER_MODE_REQ, buffer, bufferLen);
            }
            else
            {
                RetCode = Json_GMPSmartDLL.FP3_VoidAll(CurrentInterface, GetTransactionHandle(CurrentInterface), ref m_stTicket, Defines.TIMEOUT_DEFAULT);
                if (RetCode != 0)
                {
                    HandleErrorCode(RetCode);
                    return RetCode;
                }

                uint resp = GMPSmartDLL.FP3_Close(CurrentInterface, GetTransactionHandle(CurrentInterface), Defines.TIMEOUT_DEFAULT);
                if (RetCode != 0)
                {
                    HandleErrorCode(RetCode);
                    return RetCode;
                }

                DeleteTrxHandles(CurrentInterface, GetTransactionHandle(CurrentInterface));
                ClearTransactionUniqueId(CurrentInterface);

                textBox1.Text = "";
                m_txtInputData.Text = "";
                //m_comboBoxCurrency.SelectedIndex = 0;
            }

            HandleErrorCode(RetCode);
            return RetCode;
        }


        TextBox textBox1 = new TextBox(), m_txtInputData = new TextBox();


        ListBoxControl listBox1 = new ListBoxControl();

        static public UInt64 GetTransactionHandle(UInt32 InterfaceHandle)
        {
            return ACTIVE_TRX_HANDLE;
        }

        public static void DeleteTrxHandles(UInt32 hInt, UInt64 hTrx)
        {
            foreach (TreeNode Node in m_treeHandleList.Nodes)
            {
                if ((UInt32)Node.Tag == hInt)
                {
                    foreach (TreeNode Node2 in Node.Nodes)
                    {
                        if ((UInt64)Node2.Tag == hTrx)
                        {
                            Node.Nodes.Remove(Node2);
                            if (CurrentInterface == hInt)
                            {
                                if (ACTIVE_TRX_HANDLE == hTrx)
                                    ACTIVE_TRX_HANDLE = 0;
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }

        UInt32 ReloadTransaction()
        {
            UInt32 RetCode = 0;
            ST_TICKET m_stTicket = new ST_TICKET();
            UInt64 activeFlags = 0;

            RetCode = GMPSmartDLL.FP3_OptionFlags(CurrentInterface, GetTransactionHandle(CurrentInterface), ref activeFlags, Defines.GMP3_OPTION_ECHO_PRINTER | Defines.GMP3_OPTION_ECHO_ITEM_DETAILS | Defines.GMP3_OPTION_ECHO_PAYMENT_DETAILS, 0, Defines.TIMEOUT_DEFAULT);
            if (RetCode != Defines.TRAN_RESULT_OK)
                return RetCode;

            RetCode = Json_GMPSmartDLL.FP3_GetTicket(CurrentInterface, GetTransactionHandle(CurrentInterface), ref m_stTicket, Defines.TIMEOUT_DEFAULT);
            if (RetCode != Defines.TRAN_RESULT_OK)
                return RetCode;

            //            m_lstBankErrorMessage.Items.Clear();
            ComboBoxEdit listBox1 = new ComboBoxEdit();
            listBox1.Properties.Items.Clear();

            for (int i = 0; i < m_stTicket.numberOfPaymentsInThis; i++)
                listBox1.Properties.Items.Add(m_stTicket.stPayment[i].stBankPayment.stPaymentErrMessage.ErrorMsg);

            //DisplayTransaction(m_stTicket, true); //Burda Eslesmeyle gelen degerler gosterilecek

            return RetCode;
        }

        public static void ClearTransactionUniqueId(UInt32 InterfaceHandle)
        {
            if (TransactionUniqueIdList.ContainsKey(InterfaceHandle))
                Array.Clear(TransactionUniqueIdList[InterfaceHandle], 0, 24);
        }

        public void HandleErrorCode(UInt32 errorCode)
        {
            UInt64 TranHandle = 0;
            ErrorClass.DisplayErrorMessage(errorCode);

            if (errorCode == Defines.APP_ERR_GMP3_INVALID_HANDLE)
            {
                // asagıdaki sorguyu kaldırdım fis yenilenmis yukleme yapmadan devam etsin ***
                //if (MessageBox.Show(res_man.GetString("ÖKC Fisi Yenilenmis. Yüklemek ister misiniz?", "UYARI", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                return;

                // OKC'deki fis bir sebepten yeniden baslamis ve handle degismis
                // Start fonksiyonu guncel handle'i alabilir.
                UInt32 retcode = GMPSmartDLL.FP3_Start(CurrentInterface, ref TranHandle, isBackground, GetUniqueIdByInterface(CurrentInterface), 24, TsmSign, TsmSign == null ? 0 : TsmSign.Length, null, 0, 10000);
                AddTrxHandles(CurrentInterface, TranHandle, isBackground);

                if (retcode == Defines.APP_ERR_ALREADY_DONE)
                    retcode = ReloadTransaction();

                lblHata = ErrorClass.DisplayErrorMessage(retcode);
            }
        }

        public static ST_EXCHANGE[] stExchangeTable = new ST_EXCHANGE[10];
        UInt32 GetCurrency()
        {
            UInt32 retcode;
            int numberOfTotalRecordsReceived = 0;
            
            int numberOfTotalExchangeRates = 0;

            retcode = Json_GMPSmartDLL.FP3_GetExchangeTable(CurrentInterface, ref numberOfTotalExchangeRates, ref numberOfTotalRecordsReceived, ref stExchangeTable, 10);

            if (!TransactionExchangeList.ContainsKey(CurrentInterface))
                TransactionExchangeList.Add(CurrentInterface, stExchangeTable);

            if (retcode != 0)
            {
                HandleErrorCode(retcode);
                return retcode;
            }

            ComboBoxEdit m_comboBoxCurrency = new ComboBoxEdit();
            m_comboBoxCurrency.Properties.Items.Clear();
            m_comboBoxCurrency.Properties.Items.Add("949 > " + "TL 1TRL  = 1.00TL");

            for (int i = 0; i < numberOfTotalRecordsReceived; i++)
            {
                string str = "";

                switch (stExchangeTable[i].prefix)
                {
                    case "USD":
                        str += "840 > ";
                        break;
                    case "EUR":
                        str += "978 > ";
                        break;
                    case "JPY":
                        str += "826 > ";
                        break;
                    default:
                        str += "000 > ";
                        break;
                }

                str += String.Format("{0} 1{1}  = {2}.{3}TL", stExchangeTable[i].prefix
                                                , stExchangeTable[i].sign
                                                , stExchangeTable[i].rate / 100
                                                , (stExchangeTable[i].rate % 100).ToString().PadLeft(2, '0')
                                                );

                m_comboBoxCurrency.Properties.Items.Add(str);
            }

            m_comboBoxCurrency.SelectedIndex = 0;


            return retcode;
        }

        UInt32 GetDepartments()
        {
            // Button[] idDepartmenButtons = { m_btnK_017, m_btnK_018, m_btnK_019, m_btnK_020, m_btnK_021, m_btnK_022, m_btnK_023, m_btnK_024 };

            UInt32 RetCode = 0;
            byte indexOfTaxRates = 0;
            byte indexOfDepartments = 0;
            numberOfTotalRecordsReceived = 0;
            stTaxRates = new ST_TAX_RATE[8];
            stDepartments = new ST_DEPARTMENT[12];

            for (int i = 0; i < stDepartments.Length; i++)
            {
                stDepartments[i] = new ST_DEPARTMENT();
            }

            do
            {
                RetCode = Json_GMPSmartDLL.FP3_GetTaxRates_Ex(CurrentInterface, indexOfTaxRates, ref numberOfTotalTaxRates, ref numberOfTotalRecordsReceived, ref stTaxRates, 8 - indexOfTaxRates);

                if (RetCode != 0)
                    return RetCode;

                indexOfTaxRates += (byte)numberOfTotalRecordsReceived;

            } while (8 - indexOfTaxRates != 0);

            do
            {
                RetCode = Json_GMPSmartDLL.FP3_GetDepartments_Ex(CurrentInterface, indexOfDepartments, ref numberOfTotalDepartments, ref numberOfTotalRecordsReceived, ref stDepartments, 12 - indexOfDepartments);

                if (RetCode != 0)
                    return RetCode;

                indexOfDepartments += (byte)numberOfTotalRecordsReceived;

            } while (12 - indexOfDepartments != 0);

            for (int i = 0; i < indexOfTaxRates; i++)
            {
                if (i > 7)
                    continue;
                // idDepartmenButtons[i].Text = String.Format("{0}" + System.Environment.NewLine + "%{1}.{2}", stDepartments[i].szDeptName, stTaxRates[stDepartments[i].u8TaxIndex].taxRate / 100, stTaxRates[stDepartments[i].u8TaxIndex].taxRate % 100);
                ComboBoxEdit cmb_Dep = new ComboBoxEdit();
                cmb_Dep.Properties.Items.Add(String.Format("{0}" + System.Environment.NewLine + "%{1}.{2}", stDepartments[i].szDeptName, stTaxRates[stDepartments[i].u8TaxIndex].taxRate / 100, stTaxRates[stDepartments[i].u8TaxIndex].taxRate % 100));
            }

            if (!TransactionTaxRateList.ContainsKey(CurrentInterface))
                TransactionTaxRateList.Add(CurrentInterface, stTaxRates);

            if (!TransactionDepartmentList.ContainsKey(CurrentInterface))
                TransactionDepartmentList.Add(CurrentInterface, stDepartments);

            return RetCode;
        }

        public void StringToByteArray(string s, byte[] Out_byteArr, ref int Out_byteArrLen)
        {
            byte[] ba = new byte[s.Length / 2];
            for (int i = 0; i < ba.Length; i++)
            {
                string temp = s.Substring(i * 2, 2);
                ba[(ba.Length - 1) - i] = Convert.ToByte(temp, 16);
            }
            Out_byteArrLen = ba.Length;
            Array.Copy(ba, 0, Out_byteArr, 0, ba.Length);
        }

        public void StringToByteArray_Rev(string s, byte[] Out_byteArr, ref int Out_byteArrLen)
        {
            byte[] ba = new byte[s.Length / 2];
            for (int i = 0; i < ba.Length; i++)
            {
                string temp = s.Substring(i * 2, 2);
                ba[i] = Convert.ToByte(temp, 16);
            }
            Out_byteArrLen = ba.Length;
            Array.Copy(ba, 0, Out_byteArr, 0, ba.Length);
        }

        string ByteArrayToString(byte[] buffer, int bufferLen)
        {
            string str = "";
            for (int i = 0; i < bufferLen; i++)
            {
                str += buffer[i].ToString("X2");
            }
            return str;
        }


        static TreeView m_treeHandleList = new TreeView();

        public static void AddTrxHandles(UInt32 hInt, UInt64 hTrx, byte IsBackGround)
        {
            ACTIVE_TRX_HANDLE = hTrx;


            foreach (TreeNode Node in m_treeHandleList.Nodes)
            {
                if ((UInt32)Node.Tag == hInt)
                {
                    if (IsBackGround == 0)
                    {
                        TreeNode Tmp = Node.Nodes.Add(hTrx.ToString("X2") + " FG");
                        Tmp.Tag = hTrx;
                    }
                    else
                    {
                        TreeNode Tmp = Node.Nodes.Add(hTrx.ToString("X2") + " BG");
                        Tmp.Tag = hTrx;
                    }
                    break;
                }
            }

        }

        byte[] GetUniqueIdByInterface(UInt32 InterfaceHandle)
        {
            byte[] m_uniqueId = new byte[24];

            if (TransactionUniqueIdList.ContainsKey(InterfaceHandle))
                Array.Copy(TransactionUniqueIdList[InterfaceHandle], m_uniqueId, 24);

            return m_uniqueId;
        }

        public void BankaListesiAl()
        {
            if (m_tvEcho.Nodes.Count < 1)
            {
                MessageBox.Show(res_man.GetString("Aktarım Yapmadan Once Eslestirme Yapılması Gerekir"));
                return;
            }

            listBox1.Items.Clear();

            uint sonuc = 0;
            byte numberOfTotalRecords = 0;
            byte numberOfTotalRecordsReceived = 0;
            ST_PAYMENT_APPLICATION_INFO[] stPaymentApplicationInfo = new ST_PAYMENT_APPLICATION_INFO[24];
            UInt32 amount = 0;

            UInt16 currencyOfPayment = (UInt16)ECurrency.CURRENCY_TL;//(UInt16)0; //m_comboBoxCurrency.SelectedIndex;

            UInt32 retcode = Json_GMPSmartDLL.FP3_GetPaymentApplicationInfo(CurrentInterface, ref numberOfTotalRecords, ref numberOfTotalRecordsReceived, ref stPaymentApplicationInfo, 24);

            if (retcode != Defines.TRAN_RESULT_OK)
                HandleErrorCode(retcode);
            else if (numberOfTotalRecordsReceived == 0)
                MessageBox.Show(res_man.GetString("KrediKartı Odeme Uygulaması Bulunamadı"), "KrediKarti Kontrol", MessageBoxButtons.OK);
            else
            {
                ST_PAYMENT_APPLICATION_INFO[] stPaymentApplicationInfo2;

                stPaymentApplicationInfo2 = new ST_PAYMENT_APPLICATION_INFO[24];
                Array.Copy(stPaymentApplicationInfo, stPaymentApplicationInfo2, stPaymentApplicationInfo.Length);
                for (int i = 0; i < numberOfTotalRecordsReceived; i++)
                {

                    string str = "";
                    str += GMP_Tools.GetStringFromBytes(stPaymentApplicationInfo[i].name) +
                        " [" + stPaymentApplicationInfo[i].u16BKMId.ToString() + "] " +
                        " [" + stPaymentApplicationInfo[i].u16AppId.ToString("X2") + "] " +
                        " [" + stPaymentApplicationInfo[i].Status.ToString() + "] " +
                        " [" + stPaymentApplicationInfo[i].Priority.ToString() + "]";//+
                    //" [" + getAppTypeName(stPaymentApplicationInfo[i].AppType) + "]";

                    listBox1.Items.Add(str);
                }
            }
        }

       
    }
}
