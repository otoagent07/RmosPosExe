using Pos;
using Pos.Class;
using Pos.Ingenico;
using Pos.Models;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Pos
{
    public partial class Form1 : Form
    {
        SimpleTcpServer server;

        static public UInt32 CurrentInterface;
        static public UInt64 ActiveTransactionHandle;
        public UInt64 ACTIVE_TRX_HANDLE
        {
            get { return ActiveTransactionHandle; }
            set
            {
                ActiveTransactionHandle = value;
                // m_lblCurrentTransactionHandle.Text = value.ToString("X2");
                lbl_AktifHandle.Text = value.ToString("X2");
            }
        }

        Dictionary<UInt32, byte[]> TransactionUniqueIdList;
        Dictionary<UInt32, ST_DEPARTMENT[]> TransactionDepartmentList;
        Dictionary<UInt32, ST_TAX_RATE[]> TransactionTaxRateList;
        Dictionary<UInt32, ST_EXCHANGE[]> TransactionExchangeList;

        public byte isBackground = 0;

        ParserClass parsClass;
        // DisplayClass dispClass;
        ErrorClass errClass;

        bool ACTIVATE_PING = false;

        int numberOfTotalDepartments;
        int numberOfTotalTaxRates;
        int numberOfTotalRecordsReceived = 0;

        ST_DEPARTMENT[] stDepartments;
        ST_TAX_RATE[] stTaxRates;
        ST_GMP_PAIR_RESP pairingResp;

        public byte[] m_dllVersion = new byte[24];
        byte[] TsmSign = null;

        public bool BatchMode = false;

        EPaymentTypes m_PaymentType = 0;
        System.Collections.Generic.List<ST_ITEM> stItemList = new System.Collections.Generic.List<ST_ITEM>();

        //int m_type = 0;
        uint[] userMessageFlags = new uint[40];

        byte[] m_FileDirBitmap = new byte[128];
        public Form1()
        {
            InitializeComponent();
            TransactionUniqueIdList = new Dictionary<UInt32, byte[]>();
            TransactionDepartmentList = new Dictionary<UInt32, ST_DEPARTMENT[]>();
            TransactionTaxRateList = new Dictionary<UInt32, ST_TAX_RATE[]>();
            TransactionExchangeList = new Dictionary<uint, ST_EXCHANGE[]>();
            pairingResp = new ST_GMP_PAIR_RESP();
            isBackground = 0;




        }

        private void btnEslesme_Click(object sender, EventArgs e)
        {
            EslestirmeYap(true);
            this.Hide();
            taskbar.Visible = true;
            taskbar.ShowBalloonTip(3000);
        }

        public string EslestirmeYap(bool manuel = false)
        {
            string sonuc = "";
            listBox1.Items.Add(DateTime.Now.ToString());
            ST_ECHO stEcho = new ST_ECHO();
            ST_GMP_PAIR pairing = new ST_GMP_PAIR();
            UInt64 TranHandle = 0;
            UInt32 RetCode;

            lblHata.Text = "";

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
            pairing.szProcDate = lbl_Tarih.Text.Substring(0, 2) + lbl_Tarih.Text.Substring(3, 2) + lbl_Tarih.Text.Substring(6, 2);
            pairing.szProcTime = lbl_Zaman.Text.Substring(0, 2) + lbl_Zaman.Text.Substring(3, 2) + lbl_Zaman.Text.Substring(6, 2);

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
                    DialogResult dr = MessageBox.Show("ÖKC'de Tamamlanmamis Islem Var. Islemi IPTAL Etmek için CANCEL, Tekrar Yüklemek için OK 'e basin", "ÖKC'de Tamamlanmamis Islem Var. Islemi IPTAL Etmek için CANCEL, Tekrar Yüklemek için OK 'e basin", MessageBoxButtons.OKCancel);
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

        public void HandleErrorCode(UInt32 errorCode)
        {
            UInt64 TranHandle = 0;
            ErrorClass.DisplayErrorMessage(errorCode);

            if (errorCode == Defines.APP_ERR_GMP3_INVALID_HANDLE)
            {
                // asagıdaki sorguyu kaldırdım fis yenilenmis yukleme yapmadan devam etsin ***
                //if (MessageBox.Show("ÖKC Fisi Yenilenmis. Yüklemek ister misiniz?", "UYARI", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                return;

                // OKC'deki fis bir sebepten yeniden baslamis ve handle degismis
                // Start fonksiyonu guncel handle'i alabilir.
                UInt32 retcode = GMPSmartDLL.FP3_Start(CurrentInterface, ref TranHandle, isBackground, GetUniqueIdByInterface(CurrentInterface), 24, TsmSign, TsmSign == null ? 0 : TsmSign.Length, null, 0, 10000);
                AddTrxHandles(CurrentInterface, TranHandle, isBackground);

                if (retcode == Defines.APP_ERR_ALREADY_DONE)
                    retcode = ReloadTransaction();

                lblHata.Text = ErrorClass.DisplayErrorMessage(retcode);
            }
        }
        private void AddTrxHandles(UInt32 hInt, UInt64 hTrx, byte IsBackGround)
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
            listBox1.Items.Clear();

            for (int i = 0; i < m_stTicket.numberOfPaymentsInThis; i++)
                listBox1.Items.Add(m_stTicket.stPayment[i].stBankPayment.stPaymentErrMessage.ErrorMsg);

            //DisplayTransaction(m_stTicket, true); //Burda Eslesmeyle gelen degerler gosterilecek

            return RetCode;
        }

        UInt64 GetTransactionHandle(UInt32 InterfaceHandle)
        {
            return ACTIVE_TRX_HANDLE;
        }

        IngenicoSettings prm = new IngenicoSettings();
        private void Form1_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer();
            server.Delimiter = 0x13; //Enter
            server.AutoTrimStrings = true;
            server.DataReceived += server_DataReceived;
            server.StringEncoder = Encoding.UTF8;

            Pos.Ingenico.INIFile ini = new Pos.Ingenico.INIFile(Application.StartupPath + @"\Param.ini");
            prm.fisteMiktar = cevir.objToBool(ini.Read("Ayarlar", "FisteMiktar"));
            prm.fisteUrun = cevir.objToBool(ini.Read("Ayarlar", "FisteUrun"));
            prm.Dipnot = ini.Read("Ayarlar", "Dipnot");
            prm.SatisProgram = ini.Read("Ayarlar", "SatisProgram");
            prm.port = cevir.objToInt32(ini.Read("Ayarlar", "ProgramPort"), 8910);
            prm.dosyaYolu = ini.Read("Ayarlar", "DosyaYolu");
            prm.Indnot = ini.Read("Ayarlar", "Indnot");
            lbl_Port.Text = prm.port.ToString();
            lbl_host.Text = prm.host;

            prm.Server = ini.Read("Ayarlar", "SQLServer");
            prm.Database = ini.Read("Ayarlar", "SQLDatabase");
            prm.User = ini.Read("Ayarlar", "SQLUser");
            prm.Sifre = ini.Read("Ayarlar", "SQLSifre");

            AyarlarToEkran();

            TarihGuncelle();
            ParserClass.MainControls = new IngenicoConn();

            parsClass = new ParserClass();
            //          dispClass = new DisplayClass();
            errClass = new ErrorClass();


            //            DisplayClass.dispCls = this;
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

            DinlemeBaslat(prm.port);




            //tabControl1.TabPages.Remove(tabTest);
            //if (Program.disDegerler != null && Program.disDegerler.Length > 0)
            //{
            //    if (Program.disDegerler[0] == "Test") tabControl1.TabPages.Add(tabTest);
            //}

        }

        private void TarihGuncelle()
        {
            lbl_Tarih.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            lbl_Zaman.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        /*
        UInt32 GetCurrency()
        {
            UInt32 retcode;
            int numberOfTotalRecordsReceived = 0;
            ST_EXCHANGE[] stExchangeTable = new ST_EXCHANGE[10];
            int numberOfTotalExchangeRates = 0;

            retcode = Json_GMPSmartDLL.FP3_GetExchangeTable(CurrentInterface, ref numberOfTotalExchangeRates, ref numberOfTotalRecordsReceived, ref stExchangeTable, 10);

            if (!TransactionExchangeList.ContainsKey(CurrentInterface))
                TransactionExchangeList.Add(CurrentInterface, stExchangeTable);

            if (retcode != 0)
            {
                HandleErrorCode(retcode);
                return retcode;
            }

            m_comboBoxCurrency.Items.Clear();
            m_comboBoxCurrency.Items.Add("949 > " + "TL 1TRL  = 1.00TL");

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

                m_comboBoxCurrency.Items.Add(str);
            }

            m_comboBoxCurrency.SelectedIndex = 0;


            return retcode;
        }
        UInt32 GetDepartments()
        {
            Button[] idDepartmenButtons = { m_btnK_017, m_btnK_018, m_btnK_019, m_btnK_020, m_btnK_021, m_btnK_022, m_btnK_023, m_btnK_024 };

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
                idDepartmenButtons[i].Text = String.Format("{0}" + System.Environment.NewLine + "%{1}.{2}", stDepartments[i].szDeptName, stTaxRates[stDepartments[i].u8TaxIndex].taxRate / 100, stTaxRates[stDepartments[i].u8TaxIndex].taxRate % 100);
            }

            if (!TransactionTaxRateList.ContainsKey(CurrentInterface))
                TransactionTaxRateList.Add(CurrentInterface, stTaxRates);

            if (!TransactionDepartmentList.ContainsKey(CurrentInterface))
                TransactionDepartmentList.Add(CurrentInterface, stDepartments);

            return RetCode;
        }
        */
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
        string ByteArrayToString(byte[] buffer, int bufferLen)
        {
            string str = "";
            for (int i = 0; i < bufferLen; i++)
            {
                str += buffer[i].ToString("X2");
            }
            return str;
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

        void AddIntoCommandBatch(string commandName, int commandType, byte[] buffer, int bufferLen)
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

        private void DeleteTrxHandles(UInt32 hInt, UInt64 hTrx)
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
        void ClearTransactionUniqueId(UInt32 InterfaceHandle)
        {
            if (TransactionUniqueIdList.ContainsKey(InterfaceHandle))
                Array.Clear(TransactionUniqueIdList[InterfaceHandle], 0, 24);
        }

        private void m_treeHandleList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = (TreeNode)e.Node;
            if (node.Parent == null)
            {
                CurrentInterface = (UInt32)node.Tag;
                if ((node.Nodes == null) || (node.Nodes.Count == 0))
                    ACTIVE_TRX_HANDLE = 0;
                else
                    ACTIVE_TRX_HANDLE = (UInt64)node.Nodes[0].Tag;

                GetInterfaceInfo(CurrentInterface);
            }
            else
            {
                CurrentInterface = (UInt32)node.Parent.Tag;
                ACTIVE_TRX_HANDLE = (UInt64)node.Tag;

                GetInterfaceInfo(CurrentInterface);
            }

        }
        private void GetInterfaceInfo(UInt32 InterfaceHandle)
        {
            ST_INTERFACE_XML_DATA stXmlData = new ST_INTERFACE_XML_DATA();
            UInt32 Ret = Json_GMPSmartDLL.FP3_GetInterfaceXmlDataByHandle(CurrentInterface, ref stXmlData);

            byte[] ID = new byte[64];
            string Str;

            Str = "Handle : " + CurrentInterface.ToString("X8") + Environment.NewLine;
            GMPSmartDLL.FP3_GetInterfaceID(CurrentInterface, ID, (UInt32)ID.Length);
            Str += "ID : " + GMP_Tools.GetStringFromBytes(ID) + Environment.NewLine;
            if (stXmlData.IsTcpConnection == 0)
            {
                Str += "ConnectionState Type : Port" + Environment.NewLine;
                Str += "Port Name :" + stXmlData.PortName + Environment.NewLine;
                Str += "Baudrate : " + stXmlData.BaudRate.ToString() + Environment.NewLine;
                Str += "ByteSize : " + stXmlData.ByteSize.ToString() + Environment.NewLine;
                Str += "fParity : " + stXmlData.fParity.ToString() + Environment.NewLine;
                Str += "Parity : " + stXmlData.Parity.ToString() + Environment.NewLine;
                Str += "StopBit : " + stXmlData.StopBit.ToString() + Environment.NewLine;

                Str += "RetryCounter : " + stXmlData.RetryCounter.ToString() + Environment.NewLine;
            }
            else
            {
                Str += "ConnectionState Type : IP" + Environment.NewLine;
                Str += "IP :" + stXmlData.IP + "" + stXmlData.Port.ToString() + Environment.NewLine;
                Str += "IpRetryCount : " + stXmlData.IpRetryCount.ToString() + Environment.NewLine;
            }

            Str += "AckTimeOut : " + stXmlData.AckTimeOut.ToString() + Environment.NewLine;
            Str += "CommTimeOut : " + stXmlData.CommTimeOut.ToString() + Environment.NewLine;
            Str += "InterCharacterTimeOut : " + stXmlData.InterCharacterTimeOut.ToString() + Environment.NewLine;

            lbl_XmlBilgi.Text = Str;
        }

        private void btnKisimSat_Click(object sender, EventArgs e)
        {
            PluSatir ps = new PluSatir();
            ps.KisimId = int.Parse(txtKisim.Text);
            ps.Adi = txtPluAdi.Text;
            ps.Barkod = m_txtPluBarcode.Text;
            ps.Tutar = int.Parse(m_txtInputData.Text);
            DepartmentSale(ps);
        }

        private string DepartmentSale(PluSatir pluSatir)
        {
            string sonuc = "";
            UInt32 retcode;
            UInt16 currency = 949;
            bool IsVatIncludedToPrice = false;
            bool IsVatNotIncludedToPrice = false;
            UInt32 itemCount = 1;
            byte unitType = 0;//(byte)EItemUnitTypes.ITEM_KILOGRAM; 0 adet, 2 kilogram
            ST_TICKET m_stTicket = new ST_TICKET();
            ST_ITEM stItem = new ST_ITEM();


            /*
            if (m_comboBoxCurrency.Text != "")
                currency = Convert.ToUInt16(m_comboBoxCurrency.Text.Substring(0, 3));
            */
            /*
            if (m_txtInputData.Text.Contains("X"))
                itemCount = Convert.ToUInt32(m_txtInputData.Text.Substring(0, m_txtInputData.Text.IndexOf('X')));
            */

            if (prm.fisteMiktar)
            {
                if (pluSatir.Miktar <= 0) pluSatir.Miktar = 1;

                itemCount = Convert.ToUInt32(pluSatir.Miktar);
                unitType = pluSatir.BirimTipi;
                pluSatir.Tutar = pluSatir.Tutar / pluSatir.Miktar;
            }

            if (!prm.fisteUrun)
            {
                pluSatir.Adi = "";
                pluSatir.Barkod = "";
            }

            /*
            if (m_txtPluBarcode.Text == "")
                stItem.type = Defines.ITEM_TYPE_DEPARTMENT;
            else
                stItem.type = Defines.ITEM_TYPE_PLU;
            */
            stItem.type = Defines.ITEM_TYPE_DEPARTMENT; //Deneme

            stItem.subType = 0;
            stItem.deptIndex = (byte)pluSatir.KisimId; //(byte)(deptIndex - 1);
            stItem.amount = (uint)pluSatir.Tutar; //(uint)getAmount(m_txtInputData.Text);
            stItem.currency = currency;
            stItem.count = itemCount;
            stItem.unitType = unitType;
            stItem.pluPriceIndex = 0;
            stItem.countPrecition = 0;
            stItem.name = pluSatir.Adi;
            stItem.barcode = pluSatir.Barkod; //m_txtPluBarcode.Text;

            if (PromotionModel.Instance.Amount > 0)
            {
                stItem.promotion.amount = (int)PromotionModel.Instance.Amount;
                stItem.promotion.ticketMsg = PromotionModel.Instance.Message;
                stItem.promotion.type = (byte)PromotionModel.Instance.Type;
                PromotionModel.Instance.Clear();
            }


            //if (m_chcTaxFreeActive.Checked)
            if (pluSatir.DiplomatKdvli)
            {
                stItem.flag |= (uint)EItemOptions.ITEM_TAX_EXCEPTION_VAT_INCLUDED_TO_PRICE;
                IsVatIncludedToPrice = true;
            }
            //m_chcTaxFreeActive.Checked = false;

            //if (m_chcTaxFreeActive2.Checked)
            if (pluSatir.DiplomatKdvsiz)
            {
                stItem.flag |= (uint)EItemOptions.ITEM_TAX_EXCEPTION_VAT_NOT_INCLUDED_TO_PRICE;
                IsVatNotIncludedToPrice = true;
            }
            //m_chcTaxFreeActive2.Checked = false;

            if (BatchMode)
            {
                byte[] buffer = new byte[1024];
                int bufferLen = 0;

                bufferLen = Json_GMPSmartDLL.prepare_ItemSale(buffer, buffer.Length, ref stItem);
                AddIntoCommandBatch("prepare_ItemSale", Defines.GMP3_FISCAL_PRINTER_MODE_REQ, buffer, bufferLen);
            }
            else
            {
                Start:
                retcode = StartTicket(TTicketType.TProcessSale);
                if (retcode != Defines.TRAN_RESULT_OK)
                    return sonuc;

                if (IsVatIncludedToPrice == true || IsVatNotIncludedToPrice == true)
                {
                    //                    int TaxValue = stTaxRates[stDepartments[deptIndex - 1].u8TaxIndex].taxRate / 100 + stTaxRates[stDepartments[deptIndex - 1].u8TaxIndex].taxRate % 100;
                    int TaxValue = stTaxRates[stDepartments[pluSatir.KisimId].u8TaxIndex].taxRate / 100 + stTaxRates[stDepartments[pluSatir.KisimId].u8TaxIndex].taxRate % 100;

                    /*
                       if ((TaxValue == 0) && (GetReceiptOptionFlags(CurrentInterface) != 0))
                       {
                           GetInputForm gif = new GetInputForm("Exception Code", "", 2);
                           DialogResult dr2 = gif.ShowDialog();
                           if (dr2 == System.Windows.Forms.DialogResult.OK)
                               stItem.OnlineInvoiceItemExceptionCode = Convert.ToUInt16(gif.textBox1.Text);
                       }
                       */
                }


                m_stTicket.SaleInfo[0] = new ST_SALEINFO();
                retcode = Json_GMPSmartDLL.FP3_ItemSale(CurrentInterface, GetTransactionHandle(CurrentInterface), ref stItem, ref m_stTicket, Defines.TIMEOUT_DEFAULT);

                if (retcode == Defines.APP_ERR_FIS_LIMITI_ASILAMAZ)
                {
                    return "FISLIMIT";
                }

                if (retcode == Defines.APP_ERR_TICKET_HEADER_NOT_PRINTED)
                {
                    ClearTransactionUniqueId(CurrentInterface);
                    UInt64 TransHandle = GetTransactionHandle(CurrentInterface);
                    uint resp = GMPSmartDLL.FP3_Close(CurrentInterface, TransHandle, Defines.TIMEOUT_DEFAULT);
                    if (resp == Defines.TRAN_RESULT_OK)
                        DeleteTrxHandles(CurrentInterface, TransHandle);

                    goto Start;
                }
                if (retcode != 0)
                {
                    HandleErrorCode(retcode);
                    return sonuc;
                }

                DisplayTransaction(m_stTicket, false);
                HandleErrorCode(retcode);
            }

            textBox1.Text = "";
            m_txtInputData.Text = "";
            //m_comboBoxCurrency.SelectedIndex = 0;

            return sonuc;
        }

        int getAmount(string textAmount)
        {
            int amount = 0;

            if (textAmount != "")
            {
                if (textAmount.Contains("X"))
                {
                    int count = Convert.ToInt32(textAmount.Substring(0, textAmount.IndexOf('X')));
                    int index = textAmount.IndexOf('X');
                    int price = Convert.ToInt32(textAmount.Substring(index + 1));

                    amount = price;
                }
                else
                {
                    int price = Convert.ToInt32(textAmount);
                    amount = price;
                }
            }

            return amount;
        }

        UInt32 StartTicket(TTicketType ticketType)
        {
            UInt64 TranHandle = 0;
            UInt32 retcode = Defines.TRAN_RESULT_OK;

            start_again:
            if (GetTransactionHandle(CurrentInterface) == 0)
            {
                if (ticketType != TTicketType.TProcessSale)
                    ClearTransactionUniqueId(CurrentInterface);

                byte[] UserData = new byte[] { 0x74, 0x65, 0x73, 0x74, 0x64, 0x61, 0x74, 0x61 };
                retcode = GMPSmartDLL.FP3_Start(CurrentInterface, ref TranHandle, isBackground, GetUniqueIdByInterface(CurrentInterface), 24, TsmSign, TsmSign == null ? 0 : TsmSign.Length, UserData, UserData.Length, 10000);
                AddTrxHandles(CurrentInterface, TranHandle, isBackground);

                if (retcode == Defines.APP_ERR_ALREADY_DONE)
                {
                    OnBnClickedButtonVoidAll();
                    goto start_again;
                    /*
                    switch (MessageBox.Show("ÖKC'de Tamamlanmamis Islem Var. Islemi IPTAL Etmek için CANCEL, Tekrar Yüklemek için OK'e basin", "ÖKC'de Tamamlanmamis Islem Var. Islemi IPTAL Etmek için CANCEL, Tekrar Yüklemek için OK'e basin", MessageBoxButtons.OKCancel))
                    {
                        case DialogResult.OK:
                            return ReloadTransaction();
                        case DialogResult.Cancel:
                            OnBnClickedButtonVoidAll();
                            goto start_again;
                    }
                    */
                }
                else if (retcode == Defines.TRAN_RESULT_OK)
                    retcode = GMPSmartDLL.FP3_TicketHeader(CurrentInterface, GetTransactionHandle(CurrentInterface), ticketType, Defines.TIMEOUT_DEFAULT);

                if (retcode == Defines.TRAN_RESULT_OK)
                {
                    UInt64 activeFlags = 0;
                    retcode = GMPSmartDLL.FP3_OptionFlags(CurrentInterface, GetTransactionHandle(CurrentInterface), ref activeFlags, Defines.GMP3_OPTION_ECHO_PRINTER | Defines.GMP3_OPTION_ECHO_ITEM_DETAILS | Defines.GMP3_OPTION_ECHO_PAYMENT_DETAILS, 0x00000000, Defines.TIMEOUT_DEFAULT);
                }
            }

            if (retcode != Defines.TRAN_RESULT_OK)
            {
                HandleErrorCode(retcode);
                // Handle Açık kalmasın...
                UInt64 TransHandle = GetTransactionHandle(CurrentInterface);
                uint resp = GMPSmartDLL.FP3_Close(CurrentInterface, TransHandle, Defines.TIMEOUT_DEFAULT);
                if (resp == Defines.TRAN_RESULT_OK)
                    DeleteTrxHandles(CurrentInterface, TransHandle);
            }

            return retcode;
        }
        UInt32 GetReceiptOptionFlags(UInt32 CurrentInterface)
        {
            UInt32 RetCode;
            UInt64 activeFlags = 0;
            ST_TICKET m_stTicket = new ST_TICKET();

            RetCode = GMPSmartDLL.FP3_OptionFlags(CurrentInterface, GetTransactionHandle(CurrentInterface), ref activeFlags, Defines.GMP3_OPTION_ECHO_PRINTER | Defines.GMP3_OPTION_ECHO_ITEM_DETAILS | Defines.GMP3_OPTION_ECHO_PAYMENT_DETAILS, 0, Defines.TIMEOUT_DEFAULT);
            if (RetCode != Defines.TRAN_RESULT_OK)
                return RetCode;

            RetCode = Json_GMPSmartDLL.FP3_GetTicket(CurrentInterface, GetTransactionHandle(CurrentInterface), ref m_stTicket, Defines.TIMEOUT_DEFAULT);

            if (RetCode == Defines.TRAN_RESULT_OK)
                return m_stTicket.TransactionFlags & (uint)ETransactionFlags.FLG_XTRANS_ONLINE_INVOICE_PARAMETERS_SET;

            return 0;
        }
        public static void ConvertStringToHexArray(string s, ref byte[] Out_byteArr, int byteArrLen)
        {

            byte[] ba = new byte[s.Length / 2];
            for (int i = 0; i < ba.Length; i++)
            {
                string temp = s.Substring(i * 2, 2);
                ba[i] = Convert.ToByte(temp, 16);
            }
            byteArrLen = ba.Length;
            Array.Copy(ba, 0, Out_byteArr, 0, ba.Length);
        }

        //input  : "123"
        //output : 0x31, 0x32, 0x33
        public static void ConvertAscToBcdArray(string str, ref byte[] arr, int arrLen)
        {
            arrLen = str.Length;
            Array.Copy(Encoding.Default.GetBytes(str), 0, arr, 0, str.Length);
        }

        ST_TICKET sonTicket = new ST_TICKET();
        void DisplayTransaction(ST_TICKET pstTicket, bool itemDetail)
        {
            try
            {
                sonTicket = pstTicket;
                // tabControl1.SelectedTab = tabPage1;

                m_listTransaction.Items.Clear();

                string str_uniqueID = "";
                for (int i = 0; i < 24; i++)
                {
                    str_uniqueID += pstTicket.uniqueId[i].ToString("X2");
                }

                TransactionInfo(m_listTransaction, String.Format("UNIQUE ID        : " + str_uniqueID));
                TransactionInfo(m_listTransaction, String.Format("TICKET TYPE      : " + pstTicket.ticketType));
                TransactionInfo(m_listTransaction, String.Format("Z NO             : " + pstTicket.ZNo));
                TransactionInfo(m_listTransaction, String.Format("F NO             : " + pstTicket.FNo));
                TransactionInfo(m_listTransaction, String.Format("EJNO             : " + pstTicket.EJNo));
                TransactionInfo(m_listTransaction, String.Format("TRANSACTION FLAG : " + pstTicket.TransactionFlags.ToString().PadLeft(8, '0')));

                if ((pstTicket.TransactionFlags & (uint)ETransactionFlags.FLG_XTRANS_GMP3) != 0) TransactionInfo(m_listTransaction, String.Format("                : FLG_XTRANS_GMP3"));
                if ((pstTicket.TransactionFlags & (uint)ETransactionFlags.FLG_XTRANS_TICKET_HEADER_PRINTED) != 0) TransactionInfo(m_listTransaction, String.Format("                : FLG_XTRANS_TICKET_HEADER_PRINTED"));
                if ((pstTicket.TransactionFlags & (uint)ETransactionFlags.FLG_XTRANS_TICKET_TOTALS_AND_PAYMENTS_PRINTED) != 0) TransactionInfo(m_listTransaction, String.Format("                : FLG_XTRANS_TICKET_TOTALS_AND_PAYMENTS_PRINTED"));
                if ((pstTicket.TransactionFlags & (uint)ETransactionFlags.FLG_XTRANS_TICKET_FOOTER_BEFORE_MF_PRINTED) != 0) TransactionInfo(m_listTransaction, String.Format("                : FLG_XTRANS_TICKET_FOOTER_BEFORE_MF_PRINTED"));
                if ((pstTicket.TransactionFlags & (uint)ETransactionFlags.FLG_XTRANS_TICKET_FOOTER_MF_PRINTED) != 0) TransactionInfo(m_listTransaction, String.Format("                : FLG_XTRANS_TICKET_FOOTER_MF_PRINTED"));
                if ((pstTicket.TransactionFlags & (uint)ETransactionFlags.FLG_XTRANS_ONLINE_INVOICE_PARAMETERS_SET) != 0) TransactionInfo(m_listTransaction, String.Format("                : FLG_XTRANS_ONLINE_INVOICE_PARAMETERS_SET"));
                if ((pstTicket.TransactionFlags & (uint)ETransactionFlags.FLG_XTRANS_TAXFREE_PARAMETERS_SET) != 0) TransactionInfo(m_listTransaction, String.Format("                : FLG_XTRANS_TAXFREE_PARAMETERS_SET"));
                if ((pstTicket.TransactionFlags & (uint)ETransactionFlags.FLG_XTRANS_INVOICE_PARAMETERS_SET) != 0) TransactionInfo(m_listTransaction, String.Format("                : FLG_XTRANS_INVOICE_PARAMETERS_SET"));
                if ((pstTicket.TransactionFlags & (uint)ETransactionFlags.FLG_XTRANS_FULL_RCPT_CANCEL) != 0) TransactionInfo(m_listTransaction, String.Format("                : FLG_XTRANS_FULL_RCPT_CANCEL"));
                if ((pstTicket.TransactionFlags & (uint)ETransactionFlags.FLG_XTRANS_NONEY_COLLECTION_EXISTS) != 0) TransactionInfo(m_listTransaction, String.Format("                : FLG_XTRANS_NONEY_COLLECTION_EXISTS"));
                if ((pstTicket.TransactionFlags & (uint)ETransactionFlags.FLG_XTRANS_TAXLESS_ITEM_EXISTS) != 0) TransactionInfo(m_listTransaction, String.Format("                : FLG_XTRANS_TAXLESS_ITEM_EXISTS"));
                if ((pstTicket.TransactionFlags & (uint)ETransactionFlags.FLG_XTRANS_TICKETTING_EXISTS) != 0) TransactionInfo(m_listTransaction, String.Format("                : FLG_XTRANS_TICKETTING_EXISTS"));

                TransactionInfo(m_listTransaction, String.Format("OPTION FLAG      : " + pstTicket.OptionFlags.ToString().PadLeft(8, '0')));
                TransactionInfo(m_listTransaction, String.Format("ProcDate" + " : {0}/{1}/20{2}", pstTicket.szTicketDate.Substring(4, 2), pstTicket.szTicketDate.Substring(2, 2), pstTicket.szTicketDate.Substring(0, 2)));
                TransactionInfo(m_listTransaction, String.Format("ProcTime" + " : {0}:{1}:{2}", pstTicket.szTicketTime.Substring(0, 2), pstTicket.szTicketTime.Substring(2, 2), pstTicket.szTicketTime.Substring(4, 2)));
                TransactionInfo(m_listTransaction, String.Format("Total" + " : {0}.{1}", (pstTicket.TotalReceiptAmount / 100).ToString(), (pstTicket.TotalReceiptAmount % 100).ToString().PadLeft(2, '0')));
                if (pstTicket.KatkiPayiAmount != 0)
                    TransactionInfo(m_listTransaction, String.Format("MATRAHSZ        : {0}.{1}", pstTicket.KatkiPayiAmount / 100, pstTicket.KatkiPayiAmount % 100));

                TransactionInfo(m_listTransaction, String.Format("TotalTax." + " : {0}.{1}", pstTicket.TotalReceiptTax / 100, (pstTicket.TotalReceiptTax % 100).ToString().PadLeft(2, '0')));
                TransactionInfo(m_listTransaction, "ItemTable" + " : " + pstTicket.totalNumberOfItems);

                if (pstTicket.TotalReceiptDiscount != 0)
                    TransactionInfo(m_listTransaction, String.Format("TOTAL DISCOUNT  : {0}.{1}", pstTicket.TotalReceiptDiscount / 100, pstTicket.TotalReceiptDiscount % 100));

                if (pstTicket.TotalReceiptIncrement != 0)
                    TransactionInfo(m_listTransaction, String.Format("TOTAL INCREAE   : {0}.{1}", pstTicket.TotalReceiptIncrement / 100, pstTicket.TotalReceiptIncrement % 100));

                if (pstTicket.TotalReceiptItemCancel != 0)
                    TransactionInfo(m_listTransaction, String.Format("TOTAL VOID      : {0}.{1}", pstTicket.TotalReceiptItemCancel / 100, pstTicket.TotalReceiptItemCancel % 100));

                if (pstTicket.KasaAvansAmount != 0)
                    TransactionInfo(m_listTransaction, String.Format("KASA AVANS      : {0}.{1}", pstTicket.KasaAvansAmount / 100, pstTicket.KasaAvansAmount % 100));

                if (pstTicket.KasaPaymentAmount != 0)
                    TransactionInfo(m_listTransaction, String.Format("KASA PAYMENT    : {0}.{1}", pstTicket.KasaPaymentAmount / 100, pstTicket.KasaPaymentAmount % 100));

                if (pstTicket.CashBackAmount != 0)
                    TransactionInfo(m_listTransaction, String.Format("CASHBACK        : {0}.{1}", pstTicket.CashBackAmount / 100, pstTicket.CashBackAmount % 100));

                if (pstTicket.invoiceAmount != 0)
                    TransactionInfo(m_listTransaction, String.Format("INVOICE         : {0}.{1}", pstTicket.invoiceAmount / 100, pstTicket.invoiceAmount % 100));

                if (pstTicket.TaxFreeCalculated != 0)
                    TransactionInfo(m_listTransaction, String.Format("TAXFREE CALCULA : {0}.{1}", pstTicket.TaxFreeCalculated / 100, pstTicket.TaxFreeCalculated % 100));

                if (pstTicket.TaxFreeRefund != 0)
                    TransactionInfo(m_listTransaction, String.Format("TAXFREE REFUND  : {0}.{1}", pstTicket.TaxFreeRefund / 100, pstTicket.TaxFreeRefund % 100));

                if (pstTicket.TotalReceiptReversedPayment != 0)
                    TransactionInfo(m_listTransaction, String.Format("REVERSE PAYMENTS: {0} ", formatAmount(pstTicket.TotalReceiptReversedPayment, ECurrency.CURRENCY_NONE)));

                if (pstTicket.TotalReceiptIncrement != 0)
                    TransactionInfo(m_listTransaction, String.Format("INSTALLMENT COUNT   : {0}.{1}", pstTicket.TotalReceiptIncrement / 100, pstTicket.TotalReceiptIncrement % 100));

                if (pstTicket.TotalReceiptPayment != 0)
                    TransactionInfo(m_listTransaction, String.Format("PAYMENTS        : {0} ", formatAmount(pstTicket.TotalReceiptPayment, ECurrency.CURRENCY_NONE)));

                for (int i = 0; i < pstTicket.stPayment.Length; i++)
                {
                    if (pstTicket.stPayment[i] != null)
                    {
                        for (int j = 0; j < pstTicket.stPayment[i].stBankPayment.numberOfsubPayment; j++)
                        {
                            if (pstTicket.stPayment[i].stBankPayment.stBankSubPaymentInfo[j].amount != 0)
                            {
                                TransactionInfo(m_listTransaction, String.Format("BONUS NAME      : {0} ", pstTicket.stPayment[i].stBankPayment.stBankSubPaymentInfo[j].name));
                                TransactionInfo(m_listTransaction, String.Format("BONUS TYPE      : {0} ", pstTicket.stPayment[i].stBankPayment.stBankSubPaymentInfo[j].type));
                                TransactionInfo(m_listTransaction, String.Format("BONUS AMOUNT    : {0} ", pstTicket.stPayment[i].stBankPayment.stBankSubPaymentInfo[j].amount));
                            }
                        }

                        if (pstTicket.stPayment[i].stBankPayment.numberOfInstallments != 0)
                            TransactionInfo(m_listTransaction, String.Format("INSTALLMENT COUNT      : {0} ", pstTicket.stPayment[i].stBankPayment.numberOfInstallments));

                    }
                }

                TransactionInfo(m_listTransaction, "Kampanya Tablosu" + " : " + pstTicket.numberOfLoyaltyInThis);

                for (int i = 0; i < pstTicket.stLoyaltyService.Length; i++)
                {
                    if (pstTicket.stLoyaltyService[i] != null)
                    {
                        TransactionInfo(m_listTransaction, "------------");

                        TransactionInfo(m_listTransaction, String.Format("  " + "CUSTOMER ID".PadRight(20) + " : {0} ", pstTicket.stLoyaltyService[i].CustomerId));
                        TransactionInfo(m_listTransaction, String.Format("  " + "CUSTOMER ID TYPE".PadRight(20) + " : {0} ", pstTicket.stLoyaltyService[i].CustomerIdType));
                        TransactionInfo(m_listTransaction, String.Format("  " + "NAME".PadRight(20) + " : {0} ", (GMP_Tools.SetEncoding(pstTicket.stLoyaltyService[i].name))));
                        TransactionInfo(m_listTransaction, String.Format("  " + "SERVICE ID".PadRight(20) + " : {0} ", pstTicket.stLoyaltyService[i].ServiceId));
                        TransactionInfo(m_listTransaction, String.Format("  " + "DISCOUNT AMOUNT".PadRight(20) + " : {0} ", pstTicket.stLoyaltyService[i].TotalDiscountAmount));
                        TransactionInfo(m_listTransaction, String.Format("  " + "APP ID".PadRight(20) + " : {0} ", pstTicket.stLoyaltyService[i].u16AppId.ToString("X2")));
                    }
                }

                if (pstTicket.totalNumberOfPrinterLines == pstTicket.numberOfPrinterLinesInThis)
                    m_listPayment.Items.Clear();

                for (int i = 0; i < pstTicket.numberOfPrinterLinesInThis; i++)
                    TransactionInfo(m_listPayment, String.Format("{0}", pstTicket.stPrinterCopy[i].line));
                //m_listPayment.Items.Add(item27.Text);

                //for (int i = pstTicket.totalNumberOfPrinterLines - pstTicket.numberOfPrinterLinesInThis; i < pstTicket.totalNumberOfPrinterLines; i++)
                //{
                //    ListViewItem item27 = new ListViewItem(String.Format("{0}", pstTicket.stPrinterCopy[i].line));
                //    m_listPayment.Items.Add(item27.Text);
                //}


                if (itemDetail)
                    for (int i = pstTicket.totalNumberOfItems - pstTicket.numberOfItemsInThis; i < pstTicket.totalNumberOfItems; i++)
                    {
                        TransactionInfo(m_listTransaction, "Satir" + (i + 1));
                        TransactionInfo(m_listTransaction, String.Format("  " + "Name" + " : {0}", pstTicket.SaleInfo[i].Name));
                        TransactionInfo(m_listTransaction, String.Format("  " + "Barcode." + " : {0}", pstTicket.SaleInfo[i].Barcode));
                        TransactionInfo(m_listTransaction, String.Format("  " + "Amount." + " : {0}", formatAmount((uint)pstTicket.SaleInfo[i].ItemPrice, (ECurrency)pstTicket.SaleInfo[i].ItemCurrencyType)));
                        TransactionInfo(m_listTransaction, String.Format("  " + "OriginalAmount" + " : {0}", formatAmount(pstTicket.SaleInfo[i].OrigialItemAmount, (ECurrency)pstTicket.SaleInfo[i].OriginalItemAmountCurrency)));
                        TransactionInfo(m_listTransaction, String.Format("  " + "Discount." + " : {0}", formatAmount((uint)pstTicket.SaleInfo[i].DecAmount, ECurrency.CURRENCY_TL)));
                        TransactionInfo(m_listTransaction, String.Format("  " + "Save." + " : {0}", formatAmount((uint)pstTicket.SaleInfo[i].IncAmount, ECurrency.CURRENCY_TL)));
                        TransactionInfo(m_listTransaction, String.Format("  " + "Count." + " : {0}", formatCount(pstTicket.SaleInfo[i].ItemCount, pstTicket.SaleInfo[i].ItemCountPrecision, (EItemUnitTypes)pstTicket.SaleInfo[i].ItemUnitType)));
                    }

                TransactionInfo(m_listTransaction, "PaymentTable" + " : " + pstTicket.totalNumberOfPayments);

            }
            catch
            {

            }
        }

        void TransactionInfo(ListBox lst, string Item)
        {
            ListViewItem item = new ListViewItem(Item);
            lst.Items.Add(item.Text);
        }

        public string formatAmount(uint amount, ECurrency currency)
        {
            string amountStr = String.Format("{0}.{1:00}", amount / 100, amount % 100);

            switch (currency)
            {
                case ECurrency.CURRENCY_NONE:
                    break;
                case ECurrency.CURRENCY_DOLAR:
                    amountStr += " $";
                    break;
                case ECurrency.CURRENCY_EU:
                    amountStr += " €";
                    break;
                case ECurrency.CURRENCY_TL:
                    amountStr += " TL";
                    break;
                default:
                    amountStr += " ?";
                    break;
            }

            return amountStr;
        }

        string formatCount(int itemCount, byte ItemCountPrecision, EItemUnitTypes itemUnitType)
        {
            //formatCountStr += "%%ld.%%0%dd", ItemCountPrecision);
            //sprintf( cs[index], tmpFormat		
            //                            , itemCount / (long)pow((double)10, ItemCountPrecision)
            //                            , itemCount % (long)pow((double)10, ItemCountPrecision) 
            //                            );

            //switch(itemUnitType)
            //{
            //case EItemUnitTypes.ITEM_NONE:
            //    break;
            //case EItemUnitTypes.ITEM_NUMBER:
            //    strcat( cs[index], " Adt");
            //    break;
            //case EItemUnitTypes.ITEM_KILOGRAM:
            //    strcat( cs[index], " Kg");
            //    break;
            //case EItemUnitTypes.ITEM_GRAM:
            //    strcat( cs[index], " gr");
            //    break;
            //case EItemUnitTypes.ITEM_LITRE:
            //    strcat( cs[index], " lt");
            //    break;
            //}

            return itemCount.ToString();
        }

        private void btnNakit_Click(object sender, EventArgs e)
        {
            OdemeNakit(Convert.ToInt32(m_txtInputData.Text));

        }

        UInt32 GetPayment(ST_PAYMENT_REQUEST[] stPaymentRequest, int numberOfPayments)
        {
            UInt32 retcode;
            ST_TICKET m_stTicket = new ST_TICKET();

            //char display[256];
            string display = "";


            if (BatchMode)
            {
                byte[] buffer = new byte[1024];
                int bufferLen = 0;

                bufferLen = Json_GMPSmartDLL.prepare_Payment(buffer, buffer.Length, ref stPaymentRequest[0]);
                AddIntoCommandBatch("prepare_Payment", Defines.GMP3_FISCAL_PRINTER_MODE_REQ, buffer, bufferLen);
                return Defines.TRAN_RESULT_OK;
            }
            else
            {
                m_lstBankErrorMessage.Items.Clear();

                retcode = Json_GMPSmartDLL.FP3_Payment(CurrentInterface, GetTransactionHandle(CurrentInterface), ref stPaymentRequest[0], ref m_stTicket, 120000);

                for (int i = 0; i < m_stTicket.stPayment.Length; i++)
                {
                    if (m_stTicket.stPayment[i] != null)
                    {
                        if (m_stTicket.stPayment[i].stBankPayment.bankName != "")
                        {
                            m_lstBankErrorMessage.Items.Add(m_stTicket.stPayment[i].stBankPayment.bankName);
                            m_lstBankErrorMessage.Items.Add("Banking Error : " + m_stTicket.stPayment[i].stBankPayment.stPaymentErrMessage.ErrorCode + " " + m_stTicket.stPayment[i].stBankPayment.stPaymentErrMessage.ErrorMsg);
                            m_lstBankErrorMessage.Items.Add("Application Error : " + m_stTicket.stPayment[i].stBankPayment.stPaymentErrMessage.AppErrorCode + " " + m_stTicket.stPayment[i].stBankPayment.stPaymentErrMessage.AppErrorMsg);
                            m_lstBankErrorMessage.Items.Add("----------------------------------------------");
                        }
                    }
                }

                UInt32 TicketAmount = m_stTicket.TotalReceiptAmount + m_stTicket.KatkiPayiAmount;

                switch (retcode)
                {
                    case Defines.TRAN_RESULT_OK:

                        if (stPaymentRequest[0].numberOfinstallments != 0)
                            display += String.Format("TAKSIT SAYISI : {0}", stPaymentRequest[0].numberOfinstallments);

                        if (m_stTicket.KasaAvansAmount != 0)
                        {
                            display += String.Format("KASA AVANS TOTAL: {0}", formatAmount(m_stTicket.KasaAvansAmount, ECurrency.CURRENCY_TL));
                            TicketAmount = m_stTicket.KasaAvansAmount;
                        }
                        else if (m_stTicket.invoiceAmount != 0)
                        {
                            display += String.Format("INVOICE TOTAL : {0}", formatAmount(m_stTicket.invoiceAmount, ECurrency.CURRENCY_TL));
                            TicketAmount = m_stTicket.invoiceAmount;
                        }
                        else if ((TTicketType)m_stTicket.ticketType == TTicketType.TCariHesap)
                            display += String.Format("TOTAL : {0}", formatAmount(m_stTicket.stPayment[0].payAmount, ECurrency.CURRENCY_TL));
                        else
                            display += String.Format("TOTAL : {0}", formatAmount(m_stTicket.TotalReceiptAmount, ECurrency.CURRENCY_TL));

                        if (m_stTicket.CashBackAmount != 0)
                            display += String.Format(Environment.NewLine + "CASHBACK : {0}", formatAmount(m_stTicket.CashBackAmount, ECurrency.CURRENCY_TL));
                        else
                        {
                            if ((TTicketType)m_stTicket.ticketType == TTicketType.TCariHesap)
                                display += String.Format(Environment.NewLine + "REMAIN : {0}", formatAmount(m_stTicket.KasaPaymentAmount, ECurrency.CURRENCY_TL));
                            else
                                display += String.Format(Environment.NewLine + "REMAIN : {0}", formatAmount(m_stTicket.KasaPaymentAmount != 0 ? m_stTicket.KasaPaymentAmount - m_stTicket.stPayment[0].payAmount : TicketAmount - m_stTicket.TotalReceiptPayment, ECurrency.CURRENCY_TL));
                        }

                        if ((stPaymentRequest[0].typeOfPayment == (uint)EPaymentTypes.PAYMENT_BANK_CARD) || (stPaymentRequest[0].typeOfPayment == (uint)EPaymentTypes.PAYMENT_MOBILE))
                        {
                            display += String.Format(Environment.NewLine + "{0}", m_stTicket.stPayment[m_stTicket.totalNumberOfPayments - 1].stBankPayment.bankName);
                            display += String.Format(Environment.NewLine + "ONAY KODU : {0}", m_stTicket.stPayment[m_stTicket.totalNumberOfPayments - 1].stBankPayment.authorizeCode);
                            display += String.Format(Environment.NewLine + "{0}", m_stTicket.stPayment[m_stTicket.totalNumberOfPayments - 1].stBankPayment.stCard.pan);
                        }

                        if (m_stTicket.TotalReceiptPayment >= TicketAmount)
                        {
                            retcode = GMPSmartDLL.FP3_PrintTotalsAndPayments(CurrentInterface, GetTransactionHandle(CurrentInterface), Defines.TIMEOUT_DEFAULT);
                            if (retcode != Defines.TRAN_RESULT_OK && retcode != Defines.APP_ERR_ALREADY_DONE)
                                break;

                            retcode = GMPSmartDLL.FP3_PrintBeforeMF(CurrentInterface, GetTransactionHandle(CurrentInterface), Defines.TIMEOUT_DEFAULT);
                            if (retcode != Defines.TRAN_RESULT_OK && retcode != Defines.APP_ERR_ALREADY_DONE)
                                break;

                            ST_USER_MESSAGE[] stUserMessage = new ST_USER_MESSAGE[1];
                            for (int i = 0; i < stUserMessage.Length; i++)
                            {
                                stUserMessage[i] = new ST_USER_MESSAGE();
                            }

                            string mesaj = prm.Dipnot; //"Modul Yazilim.Tesekkur Ederiz";

                            stUserMessage[0].flag = Defines.PS_38 | Defines.PS_CENTER;
                            stUserMessage[0].message = mesaj;
                            stUserMessage[0].len = (byte)mesaj.Length;

                            retcode = Json_GMPSmartDLL.FP3_PrintUserMessage(CurrentInterface, GetTransactionHandle(CurrentInterface), ref stUserMessage, (ushort)stUserMessage.Length, ref m_stTicket, Defines.TIMEOUT_DEFAULT);

                            retcode = GMPSmartDLL.FP3_PrintMF(CurrentInterface, GetTransactionHandle(CurrentInterface), Defines.TIMEOUT_CARD_TRANSACTIONS);
                            if (retcode != Defines.TRAN_RESULT_OK && retcode != Defines.APP_ERR_ALREADY_DONE)
                                break;

                            ClearTransactionUniqueId(CurrentInterface);
                            UInt64 TransHandle = GetTransactionHandle(CurrentInterface);
                            retcode = GMPSmartDLL.FP3_Close(CurrentInterface, TransHandle, Defines.TIMEOUT_DEFAULT);
                            if (retcode == Defines.TRAN_RESULT_OK)
                                DeleteTrxHandles(CurrentInterface, TransHandle);
                        }

                        DisplayTransaction(m_stTicket, false);
                        break;

                    case Defines.APP_ERR_PAYMENT_NOT_SUCCESSFUL_AND_NO_MORE_ERROR_CODE:
                        DisplayTransaction(m_stTicket, false);
                        break;

                    case Defines.APP_ERR_PAYMENT_NOT_SUCCESSFUL_AND_MORE_ERROR_CODE:
                        DisplayTransaction(m_stTicket, false);

                        if (m_stTicket.totalNumberOfPayments != 0 && m_stTicket.stPayment[0] != null)
                        {
                            if ((stPaymentRequest[0].typeOfPayment == (uint)EPaymentTypes.PAYMENT_BANK_CARD) || (stPaymentRequest[0].typeOfPayment == (uint)EPaymentTypes.PAYMENT_MOBILE))
                            {
                                display += String.Format(Environment.NewLine + "{0}({1})", m_stTicket.stPayment[m_stTicket.totalNumberOfPayments - 1].stBankPayment.stPaymentErrMessage.ErrorMsg
                                                                                    , m_stTicket.stPayment[m_stTicket.totalNumberOfPayments - 1].stBankPayment.stPaymentErrMessage.ErrorCode
                                                                                    );
                                display += String.Format(Environment.NewLine + "{0}({1})", m_stTicket.stPayment[m_stTicket.totalNumberOfPayments - 1].stBankPayment.stPaymentErrMessage.AppErrorMsg
                                                                                    , m_stTicket.stPayment[m_stTicket.totalNumberOfPayments - 1].stBankPayment.stPaymentErrMessage.AppErrorCode
                                                                                    );
                            }
                        }

                        break;

                    default:
                        break;
                }

                if (display.Length != 0)
                    textBox1.Text = display;

                HandleErrorCode(retcode);

                //m_comboBoxCurrency.SelectedIndex = 0;

            }
            return retcode;
        }

        UInt32 GetCurrency()
        {
            UInt32 retcode;
            int numberOfTotalRecordsReceived = 0;
            ST_EXCHANGE[] stExchangeTable = new ST_EXCHANGE[10];
            int numberOfTotalExchangeRates = 0;

            retcode = Json_GMPSmartDLL.FP3_GetExchangeTable(CurrentInterface, ref numberOfTotalExchangeRates, ref numberOfTotalRecordsReceived, ref stExchangeTable, 10);

            if (!TransactionExchangeList.ContainsKey(CurrentInterface))
                TransactionExchangeList.Add(CurrentInterface, stExchangeTable);

            if (retcode != 0)
            {
                HandleErrorCode(retcode);
                return retcode;
            }

            m_comboBoxCurrency.Items.Clear();
            m_comboBoxCurrency.Items.Add("949 > " + "TL 1TRL  = 1.00TL");

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

                m_comboBoxCurrency.Items.Add(str);
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
                cmb_Dep.Items.Add(String.Format("{0}" + System.Environment.NewLine + "%{1}.{2}", stDepartments[i].szDeptName, stTaxRates[stDepartments[i].u8TaxIndex].taxRate / 100, stTaxRates[stDepartments[i].u8TaxIndex].taxRate % 100));
            }

            if (!TransactionTaxRateList.ContainsKey(CurrentInterface))
                TransactionTaxRateList.Add(CurrentInterface, stTaxRates);

            if (!TransactionDepartmentList.ContainsKey(CurrentInterface))
                TransactionDepartmentList.Add(CurrentInterface, stDepartments);

            return RetCode;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnParamKaydet_Click(object sender, EventArgs e)
        {
            INIFile ini = new INIFile(Application.StartupPath + @"\Param.ini");
            ini.Write("Ayarlar", "Dipnot", txt_Dipnot.Text);
            ini.Write("Ayarlar", "FisteMiktar", cevir.boolTo1_0(ch_FisteMiktar.Checked));
            ini.Write("Ayarlar", "FisteUrun", cevir.boolTo1_0(ch_FisteUrun.Checked));
            ini.Write("Ayarlar", "ProgramPort", txtPort.Text);
            ini.Write("Ayarlar", "DosyaYolu", txtDosyaYolu.Text);
            ini.Write("Ayarlar", "Indnot", txtIndnot.Text);
            ini.Write("Ayarlar", "SQLServer", txtServer.Text);
            ini.Write("Ayarlar", "SQLDatabase", txtDatabase.Text);
            ini.Write("Ayarlar", "SQLUser", txtUser.Text);
            ini.Write("Ayarlar", "SQLSifre", txtSifre.Text);

            //if (rdModul.Checked) ini.Write("Ayarlar", "SatisProgram",rdModul.Checked ? "M":(rdRmos.Checked ? "R":"M"));
            if (rdModul.Checked) ini.Write("Ayarlar", "SatisProgram", "M");
            if (rdRmos.Checked) ini.Write("Ayarlar", "SatisProgram", "R");


            MessageBox.Show("Ayarlar Kaydedildi..Ayarların Gecerli olabilmesi icin programı kapatıp tekrar acınız", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        void AyarlarToEkran()
        {
            txt_Dipnot.Text = prm.Dipnot;
            ch_FisteMiktar.Checked = prm.fisteMiktar;
            ch_FisteUrun.Checked = prm.fisteUrun;
            txtPort.Text = prm.port.ToString();
            txtDosyaYolu.Text = prm.dosyaYolu;
            txtIndnot.Text = prm.Indnot;
            txtServer.Text = prm.Server;
            txtDatabase.Text = prm.Database;
            txtUser.Text = prm.User;
            txtSifre.Text = prm.Sifre;


            if (prm.SatisProgram == "M") rdModul.Checked = true;
            if (prm.SatisProgram == "R") rdRmos.Checked = true;
        }

        private string DinlemeBitir()
        {
            string sonuc = "";
            if (server.IsStarted)
            {
                try
                {
                    server.Stop();
                    lstCihaz.Items.Add("Server Closed");

                }
                catch (Exception e)
                {

                    sonuc = e.ToString();
                }
            }

            lblCihazPortIcon.Tag = "0";
            cihazOnOff(lblCihazPortIcon.Tag.ToString());
            return sonuc;

        }
        private string DinlemeBaslat(int port)
        {
            string sonuc = "";
            lstCihaz.Items.Add("Server Starting ...");
            //            IPAddress ip = new IPAddress(long.Parse(txtHost.Text));

            IPAddress address = IPAddress.Parse(prm.host);
            byte[] bytes = address.GetAddressBytes();
            IPAddress ip = new IPAddress(bytes);

            try
            {
                server.Start(ip, prm.port);
                lstCihaz.Items.Add("Server Started");

            }
            catch (Exception e)
            {
                sonuc = e.ToString();
                MessageBox.Show("Port Acilamadi");
            }

            if (sonuc == "")
                lblCihazPortIcon.Tag = "1";

            cihazOnOff(lblCihazPortIcon.Tag.ToString());
            return sonuc;
        }

        private void cihazOnOff(string tag)
        {

            if (tag == "1")
            {
                //lblCihazPortIcon.Image = Pos.Properties.Resources.on;

            }
            else
            {
                //lblCihazPortIcon.Image = Pos.Properties.Resources.off;
            }

        }

        private void lblCihazPortIcon_DoubleClick(object sender, EventArgs e)
        {
            if (lblCihazPortIcon.Tag == "1")
            {
                if (!(MessageBox.Show("Portu Kapatmak Istediginize Emin misiniz ?", "Port Kapatma Kontrolu", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)) { return; }

                string sonuc = DinlemeBitir();

                if (sonuc == "")
                    lblCihazPortIcon.Tag = "0";
            }
            else
            {
                lbl_Port.Text = prm.port.ToString();

                string sonuc = DinlemeBaslat(prm.port);

                if (sonuc == "")
                    lblCihazPortIcon.Tag = "1";
            }

            //cihazOnOff(lblCihazPortIcon.Tag.ToString());

        }
        private bool paketDogru(string msj)
        {
            msj = msj.Trim();
            char c = (char)19; //Convert.ToChar(msj.Substring(msj.Length - 1, 1));

            //int asciiCode = (int)c;

            return msj.EndsWith(c.ToString());
        }

        private string satisYapRMOS(string yazi)
        {
            string sonuc = "";

            //if (m_tvEcho.Nodes.Count < 1) //eslestirme yapilmamissa eslestir
            //{
            //    if (!string.IsNullOrEmpty(EslestirmeYap()))
            //    {
            //        return "HATA;EslestirmeYapilamadi";
            //    }
            //}

            TarihGuncelle();

            List<OdemeSatir> YCekiodemeler = new List<OdemeSatir>();
            List<OdemeSatir> KKodemeler = new List<OdemeSatir>();
            List<OdemeSatir> Digerodemeler = new List<OdemeSatir>();
            List<PluSatir> satislar = new List<PluSatir>();
            int IndirimTutar = 0;
            int NakitTutar = 0;
            int YCekiTutar = 0;

            try
            {

                string[] veriler = yazi.Split(';');
                if (veriler.Length > 0)
                {

                    if (veriler[0].Trim() == "ESLESTIR")
                    {
                        EslestirmeYap(false);
                    }

                    if (veriler[0].Trim() == "FISIPTAL")
                    {
                        //MessageBox.Show("Fis Iptal ISlemi Gerceklesecek");
                        FisIptal();
                    }
                    if (veriler[0].Trim() == "FATURA")
                    {
                        //fatura satisi yapilacaksa
                        FaturaBilgi fat = new FaturaBilgi();
                        fat.No = veriler[1].ToString().Trim();
                        fat.Tckno = veriler[2].ToString().Trim();
                        fat.Vergino = veriler[3].ToString().Trim();
                        fat.Irsaliye = cevir.objToBool(veriler[4].ToString().Trim());
                        fat.Tipi = (byte)cevir.objToInt32(veriler[5].ToString().Trim());
                        fat.Nakit = cevir.tutarToIngc(cevir.objToDecimal(veriler[6], 0, true));
                        fat.KK = cevir.tutarToIngc(cevir.objToDecimal(veriler[7], 0, true));
                        fat.Diger = cevir.tutarToIngc(cevir.objToDecimal(veriler[8], 0, true));
                        fat.Tarih = DateTime.Now; //cevir.objToDateTime(veriler[10].ToString().Trim());


                        fat.Tutar = fat.Nakit + fat.KK + fat.Diger;
                        NakitTutar = fat.Nakit;

                        /*
                        if (NakitTutar > 0)
                        {
                            OdemeNakit(NakitTutar);
                        }
                        */
                        if (fat.KK > 0)
                        {
                            OdemeSatir odeme = new OdemeSatir();
                            odeme.Banka = 0;
                            odeme.Adi = "Kredi Karti";
                            odeme.OdemeKodu = 3;
                            odeme.Tutar = fat.KK;
                            KKodemeler.Add(odeme);
                            /*
                            uint snc = OdemeKK(odeme.Tutar, odeme.Banka);
                            if (snc != 0)
                            {
                                return "HATA;KK";
                            }
                            */
                        }
                        if (fat.Diger > 0)
                        {
                            OdemeSatir diger = new OdemeSatir();
                            diger.OdemeKodu = 15;
                            diger.Adi = "DigerOdemeler";
                            diger.Tutar = fat.Diger;
                            Digerodemeler.Add(diger);
                            //OdemeDiger(diger);
                        }

                        if (fat.Tutar > 0)
                            FaturaKes(fat);
                    }


                    if (veriler[0].Trim() == "ODEME")
                    {
                        OdemeSatir Odeme = new OdemeSatir();
                        Odeme.OdemeKodu = cevir.objToInt32(veriler[1].ToString().Trim());
                        Odeme.Tutar = cevir.tutarToIngc(cevir.objToDecimal(veriler[2].ToString().Trim(), 0, true));
                        Odeme.Banka = cevir.objToInt32(veriler[3].ToString().Trim(), 0);
                        Odeme.Adi = veriler[4].ToString().Trim();

                        if (Odeme.OdemeKodu == 1)
                        {
                            //OdemeNakit(Odeme.Tutar);
                            NakitTutar = Odeme.Tutar;
                        }
                        else
                            if (Odeme.OdemeKodu == 3)
                        {
                            KKodemeler.Add(Odeme);
                            /*
                            uint snc = OdemeKK(Odeme.Tutar, Odeme.Banka);
                            if (snc != 0)
                            {
                                return "HATA;KK";
                            }
                            */
                        }
                        else
                                if (Odeme.OdemeKodu == 4)
                        {
                            YCekiodemeler.Add(Odeme);
                            /*
                            string Sonuc = OdemeYemekCeki(Odeme.Tutar, Odeme.Banka);
                            if (!string.IsNullOrEmpty(Sonuc))
                            {
                                return "HATA;YC";
                            }
                            */
                        }
                        else
                                    if (Odeme.OdemeKodu == 21)
                        {
                            IndirimTutar = Odeme.Tutar;
                            //IndirimBindirim(1, Odeme.Tutar);
                        }
                        else
                            Digerodemeler.Add(Odeme);//OdemeDiger(Odeme);


                    }

                    if (veriler[0].Trim() == "SATIS")
                    {
                        DataTable dt = ultimateSatislar(int.Parse(veriler[1].ToString()), cevir.objToDateTime(veriler[2]), int.Parse(veriler[3].ToString()));
                        // tanımsiz yeni urun satisi
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["BA"].ToString().Trim() == "B")
                                {
                                    PluSatir ps = new PluSatir();
                                    ps.KisimId = cevir.objToInt32(dt.Rows[i]["Ykasaid"]);
                                    ps.Adi = dt.Rows[i]["Adi"].ToString().Trim();
                                    ps.Barkod = dt.Rows[i]["ybdAdi"].ToString().Trim();
                                    ps.Miktar = Convert.ToInt32(cevir.objToDecimal(dt.Rows[i]["Miktar"]));
                                    ps.Tutar = cevir.tutarToIngc(cevir.objToDecimal(dt.Rows[i]["Tutar"], 0, true));
                                    ps.BirimTipi = Convert.ToByte(dt.Rows[i]["Birim"].ToString().Trim());

                                    satislar.Add(ps);
                                }

                                if (dt.Rows[i]["BA"].ToString().Trim() == "A")
                                {
                                    OdemeSatir Odeme = new OdemeSatir();
                                    Odeme.OdemeKodu = cevir.objToInt32(dt.Rows[i]["odemeKodu"].ToString().Trim());
                                    Odeme.Tutar = cevir.tutarToIngc(cevir.objToDecimal(dt.Rows[i]["Tutar"], 0, true));
                                    Odeme.Banka = cevir.objToInt32(dt.Rows[i]["banka"], 0);
                                    Odeme.Adi = dt.Rows[i]["OdemeAdi"].ToString();

                                    switch (Odeme.OdemeKodu)
                                    {
                                        case 1://OdemeNakit(odeme.Tutar);
                                            NakitTutar += Odeme.Tutar;
                                            break;
                                        case 3://Ilerde Bankalar Birden Fazla olursa ve ayni fiste farkli banka poslarıyla kayıt cekilebilme ihtimaline karsi
                                            KKodemeler.Add(Odeme);
                                            break;
                                        case 4:
                                            YCekiTutar += Odeme.Tutar;
                                            YCekiodemeler.Add(Odeme);
                                            break;

                                        case 5: Digerodemeler.Add(Odeme); break;
                                        case 6: Digerodemeler.Add(Odeme); break;
                                        case 7: Digerodemeler.Add(Odeme); break;
                                        case 8: Digerodemeler.Add(Odeme); break;
                                        case 9: Digerodemeler.Add(Odeme); break;
                                        case 10: Digerodemeler.Add(Odeme); break;
                                        case 11: Digerodemeler.Add(Odeme); break;
                                        case 12: Digerodemeler.Add(Odeme); break;
                                        case 13: Digerodemeler.Add(Odeme); break;
                                        case 14: Digerodemeler.Add(Odeme); break;
                                        case 15: Digerodemeler.Add(Odeme); break;

                                        case 21: // IndirimBindirim(1,odeme.Tutar);
                                            IndirimTutar += Odeme.Tutar;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                        /*
                        DepartmentSale(ps);
                        */
                    }
                }

                // Satislari Gonder
                foreach (var item in satislar)
                {
                    string gelen = DepartmentSale(item);
                    if (gelen == "FISLIMIT")
                    {
                        FisIptal();
                        return "HATA;FISLIMIT";
                    }
                }

                if (IndirimTutar > 0)
                {
                    IndirimBindirim(1, IndirimTutar);
                }
                if (NakitTutar > 0)
                {
                    OdemeNakit(NakitTutar);
                }

                if (Digerodemeler.Count > 0) //Diger ODemeler 5 ile 15 arası kodlular
                {
                    foreach (var item in Digerodemeler)
                    {
                        OdemeDiger(item);
                    }
                }

                if (KKodemeler.Count > 0) //Kredi Kartlari
                {
                    foreach (var item in KKodemeler)
                    {
                        if (item.OdemeKodu == 3)
                        {
                            uint snc = OdemeKK(item.Tutar, item.Banka);
                            if (snc != 0)
                            {
                                //MessageBox.Show("Kredi Karti Cekimi Gerceklestirilemedi Hata:"+snc.ToString());
                                //SonucuGonderHata("KK;" + item.Banka.ToString() + ";" + "Kredi Karti Cekimi Gerceklestirilemedi Hata:" + snc.ToString(), dosya);
                                return "HATA;KK";
                            }
                        }
                    }
                }

                if (YCekiodemeler.Count > 0) //Yemek Cekleri
                {
                    foreach (var item in YCekiodemeler)
                    {
                        if (item.OdemeKodu == 4)
                        {
                            string Sonuc = OdemeYemekCeki(item.Tutar, item.Banka);
                            if (!string.IsNullOrEmpty(Sonuc))
                            {
                                //SonucuGonderHata("YC;" + Sonuc, dosya);
                                return "HATA;YC";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "HATA;" + ex.ToString();
            }

            FisiKapat();
            sonuc = "OK;" + sonTicket.FNo.ToString() + ";" + sonTicket.ZNo.ToString() + ";" + sonTicket.EJNo.ToString();

            return sonuc;
        }


        private DataTable ultimateSatislar(int tip, DateTime tarih, int fisno)
        {
            DataTable sonuc = new DataTable();


            try
            {
                SqlConnection conn = new SqlConnection(dbtools.connstr);
                conn.Open();


                SqlCommand cmd = new SqlCommand("spymz_yazarkasa", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.Add("@tip", SqlDbType.NVarChar).Value = tip;
                cmd.Parameters.Add("@tarih", SqlDbType.Date).Value = tarih;
                cmd.Parameters.Add("@fisno", SqlDbType.Int).Value = fisno;
                cmd.Parameters.Add("@miktarGoster", SqlDbType.NVarChar).Value = cevir.boolTo1_0(prm.fisteMiktar);
                cmd.Parameters.Add("@urunGoster", SqlDbType.NVarChar).Value = cevir.boolTo1_0(prm.fisteUrun);



                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(sonuc);


                conn.Close();
                //MessageBox.Show("Aktarim Bitmistir \r\n Kod Sinifları \r\n 91:Odeme Sekilleri \r\n 92:Departman Kısımları \r\n 93:Bankalar \r\n 94:Yemek Cekleri");

            }
            catch (Exception ex)
            {
                return null;
            }




            return sonuc;
        }
        private void satisYapMODUL(string dosya)
        {
            if (m_tvEcho.Nodes.Count < 1) //eslestirme yapilmamissa eslestir
            {
                //btnEslesme.PerformClick();
                if (!string.IsNullOrEmpty(EslestirmeYap()))
                {
                    SonucuGonderHata("EslestirmeYapilamadi", dosya);
                    return;
                }
            }

            TarihGuncelle();

            List<OdemeSatir> YCekiodemeler = new List<OdemeSatir>();
            List<OdemeSatir> KKodemeler = new List<OdemeSatir>();
            List<OdemeSatir> Digerodemeler = new List<OdemeSatir>();
            int IndirimTutar = 0;
            int NakitTutar = 0;
            int YCekiTutar = 0;

            FileStream fs = new FileStream(dosya, FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs, Encoding.GetEncoding("iso-8859-9"));

            string yazi = sw.ReadLine();
            yazi = yazi.Trim();

            string[] cekbilgiler = yazi.Split(';');
            if (cekbilgiler.Length <= 0 || cekbilgiler[0] != "CEKBILGI")
            {
                lstCihazIslem.Items.Add("Dosya Icersinden CEKBILGI satiri Gonderilmemis Islem Sonlandirilacak");
                return;
            }

            /////////
            while (yazi != null)
            {
                yazi = sw.ReadLine();
                if (string.IsNullOrEmpty(yazi)) continue;

                yazi = yazi.Trim();

                string[] veriler = yazi.Split(';');

                if (veriler.Length > 0)
                {
                    if (veriler[0].Trim() == "FISIPTAL")
                    {
                        //MessageBox.Show("Fis Iptal ISlemi Gerceklesecek");
                        FisIptal();
                    }
                    if (veriler[0].Trim() == "SATIS")
                    {
                        if (veriler[1].Trim() == "URUN")
                        {
                            if (veriler[2].Trim() == "1") //tanimli urun satisi yapilacaksa
                            {
                                // tanımli urun satisi

                            }
                            if (veriler[2].Trim() == "2") //tanimsiz, yeni urun satisi yapilacaksa
                            {
                                // tanımsiz yeni urun satisi
                                PluSatir ps = new PluSatir();
                                ps.KisimId = cevir.objToInt32(veriler[4]);
                                ps.Adi = veriler[7].ToString().Trim();
                                ps.Barkod = veriler[8].ToString().Trim();
                                ps.Miktar = Convert.ToInt32(cevir.objToDecimal(veriler[5]));
                                ps.Tutar = cevir.tutarToIngc(cevir.objToDecimal(veriler[6], 0, true));
                                ps.BirimTipi = Convert.ToByte(veriler[9].ToString().Trim());


                                string gelen = DepartmentSale(ps); ;
                                if (gelen == "FISLIMIT")
                                {
                                    FisIptal();
                                    SonucuGonderHata("FISLIMIT;Yazarkasa Fis Limiti Asildi", dosya);
                                    return;
                                }



                            }
                        }

                        if (veriler[1].Trim() == "FATURA")
                        {
                            //fatura satisi yapilacaksa
                            FaturaBilgi fat = new FaturaBilgi();
                            fat.No = veriler[2].ToString().Trim();
                            fat.Tckno = veriler[3].ToString().Trim();
                            fat.Vergino = veriler[4].ToString().Trim();
                            fat.Irsaliye = cevir.objToBool(veriler[5].ToString().Trim());
                            fat.Tipi = (byte)cevir.objToInt32(veriler[6].ToString().Trim());
                            fat.Nakit = cevir.tutarToIngc(cevir.objToDecimal(veriler[7], 0, true));
                            fat.KK = cevir.tutarToIngc(cevir.objToDecimal(veriler[8], 0, true));
                            fat.Diger = cevir.tutarToIngc(cevir.objToDecimal(veriler[9], 0, true));
                            fat.Tarih = DateTime.Now; //cevir.objToDateTime(veriler[10].ToString().Trim());


                            fat.Tutar = fat.Nakit + fat.KK + fat.Diger;
                            NakitTutar = fat.Nakit;

                            if (fat.KK > 0)
                            {
                                OdemeSatir odeme = new OdemeSatir();
                                odeme.Banka = 0;
                                odeme.Adi = "Kredi Karti";
                                odeme.OdemeKodu = 3;
                                odeme.Tutar = fat.KK;

                                KKodemeler.Add(odeme);
                            }
                            if (fat.Diger > 0)
                            {
                                OdemeSatir diger = new OdemeSatir();
                                diger.OdemeKodu = 15;
                                diger.Adi = "DigerOdemeler";
                                diger.Tutar = fat.Diger;
                            }

                            if (fat.Tutar > 0)
                                FaturaKes(fat);
                        }

                        if (veriler[1].Trim() == "ODEME")
                        {
                            if (veriler[2].Trim() == "DIGER")
                            {
                                // DIGER ODEMELERDEN YAPİLDİYSA
                                OdemeSatir Odeme = new OdemeSatir();
                                Odeme.OdemeKodu = cevir.objToInt32(veriler[3].ToString().Trim());
                                Odeme.Tutar = cevir.tutarToIngc(cevir.objToDecimal(veriler[4], 0, true));
                                Odeme.Banka = cevir.objToInt32(veriler[5], 0);
                                Odeme.Adi = veriler[6].ToString();
                                switch (Odeme.OdemeKodu)
                                {
                                    case 1://OdemeNakit(odeme.Tutar);
                                        NakitTutar += Odeme.Tutar;
                                        break;
                                    case 3://Ilerde Bankalar Birden Fazla olursa ve ayni fiste farkli banka poslarıyla kayıt cekilebilme ihtimaline karsi
                                        KKodemeler.Add(Odeme);
                                        break;
                                    case 4:
                                        YCekiTutar += Odeme.Tutar;
                                        YCekiodemeler.Add(Odeme);
                                        break;

                                    case 5: Digerodemeler.Add(Odeme); break;
                                    case 6: Digerodemeler.Add(Odeme); break;
                                    case 7: Digerodemeler.Add(Odeme); break;
                                    case 8: Digerodemeler.Add(Odeme); break;
                                    case 9: Digerodemeler.Add(Odeme); break;
                                    case 10: Digerodemeler.Add(Odeme); break;
                                    case 11: Digerodemeler.Add(Odeme); break;
                                    case 12: Digerodemeler.Add(Odeme); break;
                                    case 13: Digerodemeler.Add(Odeme); break;
                                    case 14: Digerodemeler.Add(Odeme); break;
                                    case 15: Digerodemeler.Add(Odeme); break;

                                    case 21: // IndirimBindirim(1,odeme.Tutar);
                                        IndirimTutar += Odeme.Tutar;
                                        break;
                                    default:
                                        break;
                                }

                            }


                        }
                    }

                }
                //
                //    yazi = sw.ReadLine();
            }


            if (IndirimTutar > 0)
            {
                IndirimBindirim(1, IndirimTutar);
            }
            if (NakitTutar > 0)
            {
                OdemeNakit(NakitTutar);
            }

            if (Digerodemeler.Count > 0) //Diger ODemeler 5 ile 15 arası kodlular
            {
                foreach (var item in Digerodemeler)
                {
                    OdemeDiger(item);
                }
            }

            if (KKodemeler.Count > 0) //Kredi Kartlari
            {
                foreach (var item in KKodemeler)
                {
                    if (item.OdemeKodu == 3)
                    {
                        uint snc = OdemeKK(item.Tutar, item.Banka);
                        if (snc != 0)
                        {
                            //MessageBox.Show("Kredi Karti Cekimi Gerceklestirilemedi Hata:"+snc.ToString());
                            SonucuGonderHata("KK;" + item.Banka.ToString() + ";" + "Kredi Karti Cekimi Gerceklestirilemedi Hata:" + snc.ToString(), dosya);
                            return;
                        }
                    }
                }
            }


            /* yemek cekleri birden fazla firma olabilir
            if (YCekiTutar > 0)
            {
                OdemeYemekCeki(YCekiTutar,0);
            }
            */
            if (YCekiodemeler.Count > 0) //Yemek Cekleri
            {
                foreach (var item in YCekiodemeler)
                {
                    if (item.OdemeKodu == 4)
                    {
                        string Sonuc = OdemeYemekCeki(item.Tutar, item.Banka);
                        if (!string.IsNullOrEmpty(Sonuc))
                        {
                            SonucuGonderHata("YC;" + Sonuc, dosya);
                            return;
                        }
                    }
                }
            }


            FisiKapat();


            if (1 == 1)
            {

                SonucuGonder(sonTicket, cekbilgiler, dosya);
                //        lblSatisInfo.Text += "!!!! ISLEM BASARILI !!!!!";

                //Application.Exit();
            }

            else
            {

            }
            //İSLEM SONU

            sw.Close();
            fs.Close();

        }
        private void SonucuGonder(ST_TICKET pstTicket, string[] cekbilgileri, string DosyaAdi)
        {
            string sonucDizin = prm.dosyaYolu + "cevap";
            if (!Directory.Exists(sonucDizin))
                Directory.CreateDirectory(sonucDizin);

            /*
            string str_uniqueID = "";
            for (int i = 0; i < 24; i++)
            {
                str_uniqueID += pstTicket.uniqueId[i].ToString("X2");
            }
            */

            System.IO.FileInfo ff = new FileInfo(DosyaAdi);
            string sonucAdi = ff.Name.Substring(0, ff.Name.Length - ff.Extension.Length);

            //            string ekler = ";REF|" + str_uniqueID + ";FISNO|" + pstTicket.FNo.ToString() + ";ZNO|" + pstTicket.ZNo.ToString() + ";SONUC|" + pstTicket.LastPaymentErrorMsg + ";";
            string ekler = ";REF|" + pstTicket.ZNo.ToString() + ";FISNO|" + pstTicket.FNo.ToString() + ";ZNO|" + pstTicket.ZNo.ToString() + ";SONUC|" + pstTicket.LastPaymentErrorMsg + ";";

            File.WriteAllText(sonucDizin + "\\" + sonucAdi + ".ver", string.Join(";", cekbilgileri) + ekler);
            /*
             * pstTicket.ZNo));
             * pstTicket.FNo));
             * pstTicket.EJNo))
             * 
             * 
             * 
             * 
            */
        }
        private void SonucuGonderHata(string Aciklama, string DosyaAdi)
        {
            string sonucDizin = prm.dosyaYolu + "cevap";
            if (!Directory.Exists(sonucDizin))
                Directory.CreateDirectory(sonucDizin);

            /*
            string str_uniqueID = "";
            for (int i = 0; i < 24; i++)
            {
                str_uniqueID += pstTicket.uniqueId[i].ToString("X2");
            }
            */

            System.IO.FileInfo ff = new FileInfo(DosyaAdi);
            string sonucAdi = ff.Name.Substring(0, ff.Name.Length - ff.Extension.Length);

            //            string ekler = ";REF|" + str_uniqueID + ";FISNO|" + pstTicket.FNo.ToString() + ";ZNO|" + pstTicket.ZNo.ToString() + ";SONUC|" + pstTicket.LastPaymentErrorMsg + ";";
            //string ekler = ";REF|" + pstTicket.ZNo.ToString() + ";FISNO|" + pstTicket.FNo.ToString() + ";ZNO|" + pstTicket.ZNo.ToString() + ";SONUC|" + pstTicket.LastPaymentErrorMsg + ";";

            File.WriteAllText(sonucDizin + "\\" + sonucAdi + ".ver", "HATA;" + Aciklama);
            /*
             * pstTicket.ZNo));
             * pstTicket.FNo));
             * pstTicket.EJNo))
             * 
             * 
             * 
             * 
            */
        }

        string mesajYorumla(string msj)
        {
            string sonuc = "OK;";
            //msj = msj.Substring(0, msj.Length - 1); //en sondaki kontrol karekterini kaldır
            msj = msj.Remove(msj.Length - 1);
            if (prm.SatisProgram == "M")
            {
                string[] gelen = msj.Split(';');
                if (gelen.Length > 1)
                {
                    string dosya = prm.dosyaYolu + "istek\\" + gelen[1].ToString();
                    if (!File.Exists(dosya))
                    {
                        lstCihazIslem.Items.Add("Dosya Bulunamadı:" + dosya);
                        return "HATA;Dosya Bulunamadı";
                    }
                    satisYapMODUL(dosya);
                }
            }

            if (prm.SatisProgram == "R")
            {
                string[] gelen = msj.Split(';');
                if (gelen.Length > 1)
                {
                    sonuc = satisYapRMOS(msj);
                }
            }

            return sonuc;
        }
        void server_DataReceived(object sender, SimpleTCP.Message e)
        {
            lstCihaz.Invoke((MethodInvoker)delegate ()
            {
                lstCihaz.Items.Add(e.MessageString);
                if (paketDogru(e.MessageString))
                {
                    //                    e.ReplyLine(string.Format("OK->:{0}", e.MessageString));
                    string gelenIp = e.TcpClient.Client.LocalEndPoint.ToString();

                    e.ReplyLine(mesajYorumla(e.MessageString));

                }
                else
                    e.ReplyLine(string.Format("HATA SonlandırmaYok ->:{0}", e.MessageString));


                //e.ReplyLine(string.Format("OK->:{0}", e.MessageString));
            });

        }

        private void btnAktar_Click(object sender, EventArgs e)
        {
            if (m_tvEcho.Nodes.Count < 1)
            {
                MessageBox.Show("Aktarım Yapmadan Once Eslestirme Yapılması Gerekir");
                return;
            }

            BankaListesiAl();
            YemekCekiListesiAl();
            #region RMOSUltimate Parametre Aktarim
            if (rdRmos.Checked)
            {
                if (MessageBox.Show("Yazarkasa Eslestirmesiyle Gelen Departman(Kısım) VB. Bilgileri RMOS ULTIMATE POS'a Aktarılacaktır \r\n Devam Etmek Istiyor musunuz ?", "Aktarım Kontrol", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                List<PKod> kodlar = new List<PKod>();

                PKod pkod = new PKod();
                pkod.Kod = "01";
                pkod.Ad = "PAYMENT_CASH_TL";
                pkod.Sinif = "91";
                kodlar.Add(pkod);

                pkod = new PKod();
                pkod.Kod = "03";
                pkod.Ad = "PAYMENT_BANK_CARD";
                pkod.Sinif = "91";
                kodlar.Add(pkod);

                pkod = new PKod();
                pkod.Kod = "04";
                pkod.Ad = "PAYMENT_YEMEKCEKI";
                pkod.Sinif = "91";
                kodlar.Add(pkod);

                pkod = new PKod();
                pkod.Kod = "05";
                pkod.Ad = "PAYMENT_MOBILE";
                pkod.Sinif = "91";
                kodlar.Add(pkod);

                pkod = new PKod();
                pkod.Kod = "06";
                pkod.Ad = "PAYMENT_HEDIYE_CEKI";
                pkod.Sinif = "91";
                kodlar.Add(pkod);

                pkod = new PKod();
                pkod.Kod = "07";
                pkod.Ad = "PAYMENT_IKRAM";
                pkod.Sinif = "91";
                kodlar.Add(pkod);

                pkod = new PKod();
                pkod.Kod = "08";
                pkod.Ad = "PAYMENT_ODEMESIZ";
                pkod.Sinif = "91";
                kodlar.Add(pkod);

                pkod = new PKod();
                pkod.Kod = "12";
                pkod.Ad = "PAYMENT_BANKA_TRANSFERI";
                pkod.Sinif = "91";
                kodlar.Add(pkod);

                pkod = new PKod();
                pkod.Kod = "13";
                pkod.Ad = "PAYMENT_CEK";
                pkod.Sinif = "91";
                kodlar.Add(pkod);

                pkod = new PKod();
                pkod.Kod = "14";
                pkod.Ad = "PAYMENT_ACIK_HESAP";
                pkod.Sinif = "91";
                kodlar.Add(pkod);

                pkod = new PKod();
                pkod.Kod = "15";
                pkod.Ad = "PAYMENT_DIGER";
                pkod.Sinif = "91";
                kodlar.Add(pkod);

                pkod = new PKod();
                pkod.Kod = "21";
                pkod.Ad = "INDIRIM";
                pkod.Sinif = "91";
                kodlar.Add(pkod);


                int say = 0;
                foreach (var item in cmb_Dep.Items)
                {
                    PKod pk1 = new PKod();
                    string str = item.ToString();
                    str = str.Replace("\r\n", " | ");

                    pk1.Ad = str;
                    pk1.Sinif = "92";
                    pk1.Kod = say.ToString();
                    kodlar.Add(pk1);

                    say++;
                }

                say = 0;
                foreach (var item in listBox1.Items)
                {
                    PKod pk1 = new PKod();

                    string str = item.ToString();
                    str = str.Replace("\r\n", " | ");
                    pk1.Ad = str;
                    pk1.Sinif = "93";
                    pk1.Kod = say.ToString();
                    kodlar.Add(pk1);

                    say++;
                }

                say = 0;
                foreach (var item in listBox2.Items)
                {
                    PKod pk1 = new PKod();

                    string str = item.ToString();
                    str = str.Replace("\r\n", " | ");
                    pk1.Ad = str;
                    pk1.Sinif = "94";
                    pk1.Kod = say.ToString();
                    kodlar.Add(pk1);
                    say++;
                }

                try
                {
                    SqlConnection conn = new SqlConnection(dbtools.connstr);
                    conn.Open();


                    foreach (var Pkod in kodlar)
                    {
                        SqlCommand cmd = new SqlCommand("spymz_Poskod", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Pkod_Kod", SqlDbType.NVarChar).Value = Pkod.Kod;
                        cmd.Parameters.Add("@Pkod_Ad", SqlDbType.NVarChar).Value = Pkod.Ad;
                        cmd.Parameters.Add("@Pkod_Sinif", SqlDbType.NVarChar).Value = Pkod.Sinif;
                        cmd.ExecuteNonQuery();

                    }
                    conn.Close();
                    MessageBox.Show("Aktarim Bitmistir \r\n Kod Sinifları \r\n 91:Odeme Sekilleri \r\n 92:Departman Kısımları \r\n 93:Bankalar \r\n 94:Yemek Cekleri");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Sql'e Aktarim Yapılırken Hata Olustu:" + ex.ToString());
                }

            }
            #endregion

            #region MODULPOS Parametre Aktarim
            if (rdModul.Checked)
            {
                if (MessageBox.Show("Yazarkasa Eslestirmesiyle Gelen Departman(Kısım) Bilgileri MODUL POS'a Aktarılacaktır \r\n Devam Etmek Istiyor musunuz ?", "Aktarım Kontrol", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FileStream fs = new FileStream("c:\\u\\sr\\yazarkasa\\ingenico.txt", FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("iso-8859-9"));

                    int say = 0;
                    foreach (var item in cmb_Dep.Items)
                    {
                        string str = item.ToString();
                        str = str.Replace("\r\n", " | ");
                        sw.WriteLine(string.Format("Kisim;{0};{1}", say.ToString(), str));
                        say++;
                    }

                    say = 0;
                    foreach (var item in listBox1.Items)
                    {
                        string str = item.ToString();
                        str = str.Replace("\r\n", " | ");
                        sw.WriteLine(string.Format("Banka;{0};{1}", say.ToString(), str));
                        say++;
                    }

                    say = 0;
                    foreach (var item in listBox2.Items)
                    {
                        string str = item.ToString();
                        str = str.Replace("\r\n", " | ");
                        sw.WriteLine(string.Format("YCeki;{0};{1}", say.ToString(), str));
                        say++;
                    }


                    sw.WriteLine(string.Format("Odeme;{0};{1}", "01", "PAYMENT_CASH_TL"));
                    //sw.WriteLine(string.Format("Odeme;{0};{1}", "02", "PAYMENT_CASH_CURRENCY"));
                    sw.WriteLine(string.Format("Odeme;{0};{1}", "03", "PAYMENT_BANK_CARD"));
                    sw.WriteLine(string.Format("Odeme;{0};{1}", "04", "PAYMENT_YEMEKCEKI"));
                    sw.WriteLine(string.Format("Odeme;{0};{1}", "05", "PAYMENT_MOBILE"));
                    sw.WriteLine(string.Format("Odeme;{0};{1}", "06", "PAYMENT_HEDIYE_CEKI"));
                    sw.WriteLine(string.Format("Odeme;{0};{1}", "07", "PAYMENT_IKRAM"));
                    sw.WriteLine(string.Format("Odeme;{0};{1}", "08", "PAYMENT_ODEMESIZ"));
                    //sw.WriteLine(string.Format("Odeme;{0};{1}", "09", "PAYMENT_KAPORA"));
                    //sw.WriteLine(string.Format("Odeme;{0};{1}", "10", "PAYMENT_PUAN"));
                    //sw.WriteLine(string.Format("Odeme;{0};{1}", "11", "PAYMENT_GIDER_PUSULASI"));
                    sw.WriteLine(string.Format("Odeme;{0};{1}", "12", "PAYMENT_BANKA_TRANSFERI"));
                    sw.WriteLine(string.Format("Odeme;{0};{1}", "13", "PAYMENT_CEK"));
                    sw.WriteLine(string.Format("Odeme;{0};{1}", "14", "PAYMENT_ACIK_HESAP"));
                    sw.WriteLine(string.Format("Odeme;{0};{1}", "15", "PAYMENT_DIGER"));
                    sw.WriteLine(string.Format("Odeme;{0};{1}", "21", "INDIRIM"));




                    sw.Close();
                    fs.Close();
                    ModulProgramCalistir(@"c:\u\sr\obj\run\wrun32.exe -c c:\u\sr\obj\run\cblconfi c:\u\sr\obj\pos2007\ingParam.rmo");
                    MessageBox.Show("Aktarım Tamamlanmıstır");
                }
            }
            #endregion

        }

        public void ModulProgramCalistir(string komut)
        {
            if (komut == "") return;

            System.Diagnostics.Process prc = new System.Diagnostics.Process();
            prc.StartInfo.UseShellExecute = false;

            prc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            prc.StartInfo.CreateNoWindow = true; //not diplay a windows


            // yazilan komuttan program yolunu exe yolunu ve parametrenin cikarilmasi islemi
            string hamExe = "", ExeYolu = "", ExeParam = "", text1 = "";

            int exeyeri = komut.IndexOf('.'); //ilk once noktanin yerine kadar olan kısmı kes
            if (exeyeri > 0)
            {
                hamExe = komut.Substring(0, exeyeri + 4);
                text1 = komut.Remove(exeyeri, komut.Length - exeyeri);
                if (text1.LastIndexOf('\\') > 0) // .exe den onceki exenin adini cikarip calisma yolunu bulmak icin
                {
                    ExeYolu = text1.Substring(0, text1.LastIndexOf('\\'));
                }
                if ((komut.Length - exeyeri) > 4) // exe nin yanında parametre var demekki
                {
                    ExeParam = komut.Substring(exeyeri + 4, komut.Length - exeyeri - 4).Trim();
                }
            }

            //komutParam = @"c:\u\sr\obj\run\wrun32.exe -c c:\u\sr\obj\run\cblconfi c:\u\sr\obj\otel2005\menu-c.rmo";

            prc.StartInfo.FileName = hamExe;
            if (ExeParam != "")
                prc.StartInfo.Arguments = ExeParam; //@"c:\u\sr\obj\run\wrun32.exe -c c:\u\sr\obj\run\cblconfi c:\u\sr\obj\otel2005\menu-c.rmo";
            if (ExeYolu != "")
                prc.StartInfo.WorkingDirectory = ExeYolu; //@"c:\u\sr\obj\run";

            prc.Start();

            prc.WaitForExit();

            //MessageBox.Show("Komut CAgrildi"+komut);
            /*   Process p = new Process();
             *   p.StartInfo.UseShellExecute = false;
                 p.StartInfo.RedirectStandardOutput = true;
                 p.StartInfo.FileName = "C:\\SMARTTEKDISK\\CARDREADER\\SmartCardServerUSB.exe";
                 p.StartInfo.Arguments = "GETCARDNO";
                 p.Start();
                 kartno = p.StandardOutput.ReadLine().Trim();
                 p.WaitForExit();
              
            */

            /*
             * Process p = new Process();
         p.StartInfo.FileName = "cmd.exe";
         p.StartInfo.Arguments = "/c dir *.cs";
         p.StartInfo.UseShellExecute = false;
         p.StartInfo.RedirectStandardOutput = true;
         p.Start();

         string output = p.StandardOutput.ReadToEnd();
         p.WaitForExit();

         Console.WriteLine("Output:");
         Console.WriteLine(output);   
            */
        }
        private void OdemeNakit(int tutar)
        {
            UInt32 amount = 0;
            UInt16 currencyOfPayment = 0; //(UInt16)m_comboBoxCurrency.SelectedIndex;
            ST_PAYMENT_REQUEST[] stPaymentRequest = new ST_PAYMENT_REQUEST[1];

            /*
            if (m_comboBoxCurrency.SelectedIndex == -1)
                m_comboBoxCurrency.SelectedIndex = 0;
            */

            /*
            if (currencyOfPayment == (UInt16)ECurrency.CURRENCY_NONE)
                currencyOfPayment = (UInt16)ECurrency.CURRENCY_TL;
            */
            currencyOfPayment = (UInt16)ECurrency.CURRENCY_TL;
            /*
            if (m_txtInputData.Text.Length > 0)
            {
                amount = (uint)getAmount(m_txtInputData.Text);
                m_txtInputData.Text = "";
            }
            */
            amount = (uint)tutar;//getAmount(m_txtInputData.Text);

            for (int i = 0; i < stPaymentRequest.Length; i++)
            {
                stPaymentRequest[i] = new ST_PAYMENT_REQUEST();
            }

            stPaymentRequest[0].typeOfPayment = (uint)EPaymentTypes.PAYMENT_CASH_TL;
            stPaymentRequest[0].subtypeOfPayment = 0;
            stPaymentRequest[0].payAmount = amount;
            stPaymentRequest[0].payAmountCurrencyCode = currencyOfPayment;

            GetPayment(stPaymentRequest, 1);
        }
        private string OdemeYemekCeki(int tutar, int yceki)
        {
            string sonuc = "";
            byte NumberOfTotalRecord = 24;
            byte NumberOfTotalRecordReceived = 0;

            ST_PAYMENT_APPLICATION_INFO[] StPaymentApplicationInfo = new ST_PAYMENT_APPLICATION_INFO[24];
            for (int i = 0; i < StPaymentApplicationInfo.Length; i++)
            {
                StPaymentApplicationInfo[i] = new ST_PAYMENT_APPLICATION_INFO();
            }

            uint retcode = Json_GMPSmartDLL.FP3_GetVasApplicationInfo(CurrentInterface, ref NumberOfTotalRecord, ref NumberOfTotalRecordReceived, ref StPaymentApplicationInfo, (ushort)EVasType.TLV_OKC_ASSIST_VAS_TYPE_YEMEKCEKI);

            if (retcode != Defines.TRAN_RESULT_OK)
            {
                HandleErrorCode(retcode);
                sonuc = retcode.ToString();
                return sonuc;
            }
            else
                if (NumberOfTotalRecordReceived == 0)
            {
                // MessageBox.Show("ÖKC Üzerinde Ödeme Uygulanaması Bulunamadı", "HATA", MessageBoxButtons.OK);
                sonuc = "YemekCeki Uygulamasi Yok";
                return sonuc;
            }
            else
            {
                /*
                PaymentAppForm paf = new PaymentAppForm(NumberOfTotalRecordReceived, StPaymentApplicationInfo);
                DialogResult dr = paf.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK)
                    return;
                */
                UInt32 amount = 0;
                /*
                if (m_comboBoxCurrency.SelectedIndex == -1)
                {
                    m_comboBoxCurrency.SelectedIndex = 0;
                }
                UInt16 currencyOfPayment = (UInt16)m_comboBoxCurrency.SelectedIndex;

                if (currencyOfPayment == (UInt16)ECurrency.CURRENCY_NONE)
                    currencyOfPayment = (UInt16)ECurrency.CURRENCY_TL;


                if (m_txtInputData.Text.Length > 0)
                {
                    amount = (uint)getAmount(m_txtInputData.Text);
                    m_txtInputData.Text = "";
                }
                */

                amount = (UInt32)tutar;

                UInt16 currencyOfPayment = (UInt16)ECurrency.CURRENCY_TL;

                ST_PAYMENT_REQUEST[] stPaymentRequest = new ST_PAYMENT_REQUEST[1];
                for (int i = 0; i < stPaymentRequest.Length; i++)
                {
                    stPaymentRequest[i] = new ST_PAYMENT_REQUEST();
                }

                stPaymentRequest[0].typeOfPayment = (uint)EPaymentTypes.PAYMENT_YEMEKCEKI;
                stPaymentRequest[0].subtypeOfPayment = 0;
                stPaymentRequest[0].payAmount = amount;
                stPaymentRequest[0].payAmountCurrencyCode = (ushort)ECurrency.CURRENCY_TL;
                /*
                if (paf.pstPaymentApplicationInfoSelected.u16BKMId.Equals(null))
                    stPaymentRequest[0].bankBkmId = 0;
                else
                    stPaymentRequest[0].bankBkmId = paf.pstPaymentApplicationInfoSelected.u16AppId;
                */

                ST_PAYMENT_APPLICATION_INFO pstPaymentApplicationInfoSelected = StPaymentApplicationInfo[yceki];
                //                stPaymentRequest[0].bankBkmId = pstPaymentApplicationInfoSelected.u16AppId;
                if (pstPaymentApplicationInfoSelected.u16BKMId.Equals(null))
                    stPaymentRequest[0].bankBkmId = 0;
                else
                    stPaymentRequest[0].bankBkmId = pstPaymentApplicationInfoSelected.u16AppId;


                stPaymentRequest[0].numberOfinstallments = 0;

                GetPayment(stPaymentRequest, 1);
            }

            return sonuc;
        }

        //Banking Checkbox larini burda bool tipinde tanimladim
        bool m_chcManualPanEntryNotAllowed,
          m_chcLoyaltyPointNotSupported,
          m_chcAllInputFromEcr,
          m_chcDoNotAskForMissingLoyaltyPoint,
          m_chcAuthorisationForInvoicePayment,
          m_chcSaleWithoutCampaign = false;

        private uint OdemeKK(int tutar, int banka)
        {
            uint sonuc = 0;
            byte numberOfTotalRecords = 0;
            byte numberOfTotalRecordsReceived = 0;
            ST_PAYMENT_APPLICATION_INFO[] stPaymentApplicationInfo = new ST_PAYMENT_APPLICATION_INFO[24];
            UInt32 amount = 0;
            /*
            if (m_comboBoxCurrency.SelectedIndex == -1)
            {
                if (m_comboBoxCurrency.Items.Count > 0)
                    m_comboBoxCurrency.SelectedIndex = 0;
                else
                    break;
            }
            */
            UInt16 currencyOfPayment = (UInt16)ECurrency.CURRENCY_TL;//(UInt16)0; //m_comboBoxCurrency.SelectedIndex;

            UInt32 retcode = Json_GMPSmartDLL.FP3_GetPaymentApplicationInfo(CurrentInterface, ref numberOfTotalRecords, ref numberOfTotalRecordsReceived, ref stPaymentApplicationInfo, 24);

            if (retcode != Defines.TRAN_RESULT_OK)
                HandleErrorCode(retcode);
            else if (numberOfTotalRecordsReceived == 0)
                MessageBox.Show("KrediKartı Odeme Uygulaması Bulunamadı", "KrediKarti Kontrol", MessageBoxButtons.OK);
            else
            {
                ST_PAYMENT_REQUEST[] stPaymentRequest = new ST_PAYMENT_REQUEST[1];
                for (int i = 0; i < stPaymentRequest.Length; i++)
                {
                    stPaymentRequest[i] = new ST_PAYMENT_REQUEST();
                }

                /*
                PaymentAppForm paf = new PaymentAppForm(numberOfTotalRecordsReceived, stPaymentApplicationInfo);
                DialogResult dr = paf.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK)
                    return;
                
                if (currencyOfPayment == (UInt16)ECurrency.CURRENCY_NONE)
                    currencyOfPayment = (UInt16)ECurrency.CURRENCY_TL;
                */
                currencyOfPayment = (UInt16)ECurrency.CURRENCY_TL;

                /*
                if (m_txtInputData.Text.Length != 0)
                {
                    amount = (uint)getAmount(m_txtInputData.Text);
                    m_txtInputData.Text = "";

                }
                */
                amount = (uint)tutar;
                /*
                PaymentTypeForm ptf = new PaymentTypeForm();
                DialogResult ptfDr = ptf.ShowDialog();
                if (ptfDr != System.Windows.Forms.DialogResult.OK)
                    return;
                
                switch (PaymentTypeForm.PaymentTypeIndex)
                        */
                int islemSekli = 0;//POSTAN Cek Gelen Ekrandaki Birinci Secenek = SELECT TRANSACTION TYPE ON DEVICE
                                   /*"SELECT TRANSACTION TYPE ON DEVICE";
                                    *"SALE"; 
                                    *"INSTALMENT SALE"; 
                                    *"BONUS SALE"; 
                                    * 
                                   */

                switch (islemSekli)
                {
                    case 0:
                        stPaymentRequest[0].subtypeOfPayment = Defines.PAYMENT_SUBTYPE_PROCESS_ON_POS;
                        break;
                    /*
                case 1:
                    stPaymentRequest[0].subtypeOfPayment = Defines.PAYMENT_SUBTYPE_SALE;
                    stPaymentRequest[0].BankPaymentUniqueId = GenerateUniqueId();
                    break;
                    */
                    case 2:
                        stPaymentRequest[0].subtypeOfPayment = Defines.PAYMENT_SUBTYPE_INSTALMENT_SALE;
                        break;
                    case 3:
                        stPaymentRequest[0].subtypeOfPayment = Defines.PAYMENT_SUBTYPE_LOYALTY_PUAN;
                        break;
                    default:
                        sonuc = 1;
                        return sonuc;
                }

                int numberOfinstallments = 0;
                int bonusAmount = 0;

                /*
                GetInputForm gif;

                if ((stPaymentRequest[0].subtypeOfPayment == Defines.PAYMENT_SUBTYPE_PROCESS_ON_POS) || (stPaymentRequest[0].subtypeOfPayment == Defines.PAYMENT_SUBTYPE_INSTALMENT_SALE))
                {
                    do
                    {

                        gif = new GetInputForm(Localization.InstalmentCount, "0", 2);
                        DialogResult dr2 = gif.ShowDialog();
                        if (dr2 != System.Windows.Forms.DialogResult.OK)
                            return;

                        Int32.TryParse(gif.textBox1.Text, out numberOfinstallments);
                    } while (numberOfinstallments > 9);
                }
                */

                numberOfinstallments = 0; //yilmaz taksit sayisi

                if (stPaymentRequest[0].subtypeOfPayment == Defines.PAYMENT_SUBTYPE_LOYALTY_PUAN)
                {
                    // partial bonus is not supported. If payment has been started as Loyalty, bonusAmount must equal to Amount.
                    bonusAmount = (int)amount;
                    //do
                    //{
                    //    gif = new GetInputForm(Localization.BonusAmount, amount.ToString(), 2);
                    //    DialogResult dr3 = gif.ShowDialog();
                    //    if (dr3 != System.Windows.Forms.DialogResult.OK)
                    //        return;
                    //    Int32.TryParse(gif.textBox1.Text, out bonusAmount);
                    //} while (amount != bonusAmount);
                }

                stPaymentRequest[0].typeOfPayment = (uint)EPaymentTypes.PAYMENT_BANK_CARD;
                stPaymentRequest[0].payAmount = amount;
                stPaymentRequest[0].payAmountBonus = (uint)bonusAmount;

                stPaymentRequest[0].payAmountCurrencyCode = currencyOfPayment;
                //if (paf.pstPaymentApplicationInfoSelected.u16BKMId.Equals(null))
                //    stPaymentRequest[0].bankBkmId = 0;

                /*ymz
                if (paf.pstPaymentApplicationInfoSelected == null)
                    stPaymentRequest[0].bankBkmId = 0;
                else
                    stPaymentRequest[0].bankBkmId = paf.pstPaymentApplicationInfoSelected.u16BKMId;
                */

                ST_PAYMENT_APPLICATION_INFO pstPaymentApplicationInfoSelected = stPaymentApplicationInfo[banka];
                stPaymentRequest[0].bankBkmId = pstPaymentApplicationInfoSelected.u16BKMId;


                //                stPaymentRequest[0].bankBkmId = paf.pstPaymentApplicationInfoSelected.u16BKMId;

                stPaymentRequest[0].numberOfinstallments = (ushort)numberOfinstallments;

                stPaymentRequest[0].transactionFlag = 0x00000000;
                if (m_chcManualPanEntryNotAllowed)
                    stPaymentRequest[0].transactionFlag |= Defines.BANK_TRAN_FLAG_MANUAL_PAN_ENTRY_NOT_ALLOWED;
                if (m_chcLoyaltyPointNotSupported)
                    stPaymentRequest[0].transactionFlag |= Defines.BANK_TRAN_FLAG_LOYALTY_POINT_NOT_SUPPORTED_FOR_TRANS;
                if (m_chcAllInputFromEcr)
                    stPaymentRequest[0].transactionFlag |= Defines.BANK_TRAN_FLAG_ALL_INPUT_FROM_EXTERNAL_SYSTEM;
                if (m_chcDoNotAskForMissingLoyaltyPoint)
                    stPaymentRequest[0].transactionFlag |= Defines.BANK_TRAN_FLAG_DO_NOT_ASK_FOR_MISSING_LOYALTY_POINT;
                if (m_chcAuthorisationForInvoicePayment)
                    stPaymentRequest[0].transactionFlag |= Defines.BANK_TRAN_FLAG_AUTHORISATION_FOR_INVOICE_PAYMENT;
                if (m_chcSaleWithoutCampaign)
                    stPaymentRequest[0].transactionFlag |= Defines.BANK_TRAN_FLAG_SALE_WITHOUT_CAMPAIGN;

                stPaymentRequest[0].rawData = Encoding.Default.GetBytes("RawData from external application for the payment application");
                stPaymentRequest[0].rawDataLen = (ushort)stPaymentRequest[0].rawData.Length;

                sonuc = GetPayment(stPaymentRequest, 1);
            }

            return sonuc;
        }

        private string KampanyaName;
        private string KampanyaBkmId;
        private string KampanyaCustomerId;
        private string KampanyaServiceId;

        private void OdemeDiger(OdemeSatir odeme)
        {
            uint amount = (uint)odeme.Tutar;
            UInt16 currencyOfPayment = 949;



            ST_PAYMENT_REQUEST[] stPaymentRequest = new ST_PAYMENT_REQUEST[1];
            for (int i = 0; i < stPaymentRequest.Length; i++)
            {
                stPaymentRequest[i] = new ST_PAYMENT_REQUEST();
            }

            if (string.IsNullOrEmpty(odeme.Adi)) odeme.Adi = "DigerOdemeler";

            stPaymentRequest[0].typeOfPayment = (uint)BulPaymentType(odeme.OdemeKodu);//m_PaymentType;
            stPaymentRequest[0].subtypeOfPayment = 0;
            stPaymentRequest[0].payAmount = amount;
            stPaymentRequest[0].payAmountCurrencyCode = currencyOfPayment;
            stPaymentRequest[0].paymentName = "* " + odeme.Adi; //"INGENICO";

            stPaymentRequest[0].bankBkmId = Convert.ToUInt16(KampanyaBkmId);
            stPaymentRequest[0].numberOfinstallments = 0;
            stPaymentRequest[0].LoyaltyServiceId = Convert.ToUInt16(KampanyaServiceId);
            stPaymentRequest[0].LoyaltyCustomerId = KampanyaCustomerId;

            GetPayment(stPaymentRequest, 1);

        }

        private EPaymentTypes BulPaymentType(int odemeKodu)
        {
            EPaymentTypes sonuc = 0;
            switch (odemeKodu)
            {
                case 1: sonuc = EPaymentTypes.PAYMENT_CASH_TL; break;
                case 2: sonuc = EPaymentTypes.PAYMENT_CASH_TL; break;
                case 3: sonuc = EPaymentTypes.PAYMENT_BANK_CARD; break;
                case 4: sonuc = EPaymentTypes.PAYMENT_YEMEKCEKI; break;
                case 5: sonuc = EPaymentTypes.PAYMENT_MOBILE; break;
                case 6: sonuc = EPaymentTypes.PAYMENT_HEDIYE_CEKI; break;
                case 7: sonuc = EPaymentTypes.PAYMENT_IKRAM; break;
                case 8: sonuc = EPaymentTypes.PAYMENT_ODEMESIZ; break;
                case 9: sonuc = EPaymentTypes.PAYMENT_KAPORA; break;
                case 10: sonuc = EPaymentTypes.PAYMENT_PUAN; break;
                case 11: sonuc = EPaymentTypes.PAYMENT_GIDER_PUSULASI; break;
                case 12: sonuc = EPaymentTypes.PAYMENT_BANKA_TRANSFERI; break;
                case 13: sonuc = EPaymentTypes.PAYMENT_CEK; break;
                case 14: sonuc = EPaymentTypes.PAYMENT_ACIK_HESAP; break;
                case 15: sonuc = EPaymentTypes.PAYMENT_DIGER; break;
                default:
                    break;
            }

            return sonuc;
        }

        //int m_type = 0;
        void IndirimBindirim(int m_type, int TutarOran)
        {
            UInt16 m_itemNo = 0xFFFF; //Tüm Fiste Uygulaması icin

            UInt32 retcode = Defines.DLL_RETCODE_FAIL;
            ST_TICKET m_stTicket = new ST_TICKET();
            /*
            if (m_txtInputData.Text == "")
            {
                MessageBox.Show("Lütfen Bir Oran veya Tutar Girin...");
                return;
            }
            */
            int changedAmount = TutarOran; //(int)getAmount(m_txtInputData.Text);

            if (m_itemNo != 0xFFFF)
                m_itemNo--;

            string str1 = "";

            if (BatchMode)
            {/*
                byte[] buffer = new byte[1024];
                int bufferLen = 0;
               
                if (m_type == 0)    //amount Increase{
                {
                    bufferLen = GMPSmartDLL.prepare_Plus(buffer, buffer.Length, changedAmount, m_itemNo);
                    AddIntoCommandBatch("prepare_Plus", Defines.GMP3_FISCAL_PRINTER_MODE_REQ, buffer, bufferLen);
                    str1 = "Tutar Artış"; //Localization.AmountIncrease;
                }
                else if (m_type == 1)//amount Decrease
                {
                    bufferLen = GMPSmartDLL.prepare_Minus(buffer, buffer.Length, changedAmount, m_itemNo);
                    AddIntoCommandBatch("prepare_Minus", Defines.GMP3_FISCAL_PRINTER_MODE_REQ, buffer, bufferLen);
                }
                else if (m_type == 2)//percent Increase
                {
                    bufferLen = GMPSmartDLL.prepare_Inc(buffer, buffer.Length, Convert.ToByte(m_txtInputData.Text), m_itemNo);
                    AddIntoCommandBatch("prepare_Inc", Defines.GMP3_FISCAL_PRINTER_MODE_REQ, buffer, bufferLen);
                }
                else if (m_type == 3)//percent Decrease
                {
                    bufferLen = GMPSmartDLL.prepare_Dec(buffer, buffer.Length, Convert.ToByte(m_txtInputData.Text), m_itemNo);
                    AddIntoCommandBatch("prepare_Dec", Defines.GMP3_FISCAL_PRINTER_MODE_REQ, buffer, bufferLen);
                }
                */
            }
            else
            {
                /*
                GetInputForm gif = new GetInputForm("Kullanici Mesaji", "", 1);
                gif.ShowDialog();
                */
                if (m_type == 0)    //amount Increase{ Tutar Artır
                {
                    retcode = Json_GMPSmartDLL.FP3_Plus(CurrentInterface, GetTransactionHandle(CurrentInterface), changedAmount, prm.Indnot, ref m_stTicket, m_itemNo, Defines.TIMEOUT_DEFAULT);
                    str1 = "Tutar Artış";// Localization.AmountIncrease;
                }
                else if (m_type == 1)//amount Decrease Tutar Indir
                {
                    retcode = Json_GMPSmartDLL.FP3_Minus(CurrentInterface, GetTransactionHandle(CurrentInterface), changedAmount, prm.Indnot, ref m_stTicket, m_itemNo, Defines.TIMEOUT_DEFAULT);
                }
                else if (m_type == 2)//percent Increase Yuzde Artır
                {

                    retcode = Json_GMPSmartDLL.FP3_Inc(CurrentInterface, GetTransactionHandle(CurrentInterface), Convert.ToByte(TutarOran), prm.Indnot, ref m_stTicket, m_itemNo, ref changedAmount, Defines.TIMEOUT_DEFAULT);
                }
                else if (m_type == 3)//percent Decrease Yuzde Indir
                {
                    retcode = Json_GMPSmartDLL.FP3_Dec(CurrentInterface, GetTransactionHandle(CurrentInterface), Convert.ToByte(TutarOran), prm.Indnot, ref m_stTicket, m_itemNo, ref changedAmount, Defines.TIMEOUT_DEFAULT);
                }

                if (retcode != 0)
                {
                    HandleErrorCode(retcode);
                    return;
                }

                DisplayTransaction(m_stTicket, false);

                if (m_itemNo == 0xFFFF)
                {
                    textBox1.Text = String.Format("{0} ({1})" + Environment.NewLine + "{2}" + Environment.NewLine + "{3} {4}"
                                                    , str1
                                                    , "Tüm Fiste"//Localization.AllTicket
                                                    , formatAmount((uint)changedAmount, ECurrency.CURRENCY_TL)
                                                    , "Ara Toplam"//Localization.SubTotal
                                                    , formatAmount(m_stTicket.TotalReceiptAmount, ECurrency.CURRENCY_TL)
                                                    );
                }
                else
                {
                    textBox1.Text = String.Format("{0} ({1} {2})" + Environment.NewLine + "+{3}" + Environment.NewLine + "{4} X {5} {6}"
                                                    , str1
                                                    , "ITEM"//Localization.Item
                                                    , m_itemNo
                                                    , formatAmount((uint)changedAmount, ECurrency.CURRENCY_TL)
                                                    , formatCount(m_stTicket.SaleInfo[m_itemNo].ItemCount, m_stTicket.SaleInfo[m_itemNo].ItemCountPrecision, (EItemUnitTypes)m_stTicket.SaleInfo[m_itemNo].ItemUnitType)
                                                    , m_stTicket.SaleInfo[m_itemNo].Name
                                                    , formatAmount((uint)m_stTicket.SaleInfo[m_itemNo].ItemPrice, (ECurrency)m_stTicket.SaleInfo[m_itemNo].ItemCurrencyType)
                                                    );
                }

                m_txtInputData.Text = "";
                HandleErrorCode(retcode);

            }
        }
        void FisiKapat()
        {
            UInt32 retcode;

            if (GetTransactionHandle(CurrentInterface) == 0)
                return;

            retcode = GMPSmartDLL.FP3_PrintTotalsAndPayments(CurrentInterface, GetTransactionHandle(CurrentInterface), Defines.TIMEOUT_DEFAULT);
            if (retcode != Defines.TRAN_RESULT_OK)
                goto Exit;

            retcode = GMPSmartDLL.FP3_PrintBeforeMF(CurrentInterface, GetTransactionHandle(CurrentInterface), Defines.TIMEOUT_DEFAULT);
            if (retcode != Defines.TRAN_RESULT_OK)
                goto Exit;

            retcode = GMPSmartDLL.FP3_PrintMF(CurrentInterface, GetTransactionHandle(CurrentInterface), Defines.TIMEOUT_DEFAULT);
            if (retcode != Defines.TRAN_RESULT_OK)
                goto Exit;

            ClearTransactionUniqueId(CurrentInterface);

            UInt64 TransHandle = GetTransactionHandle(CurrentInterface);
            retcode = GMPSmartDLL.FP3_Close(CurrentInterface, TransHandle, Defines.TIMEOUT_DEFAULT);
            if (retcode == Defines.TRAN_RESULT_OK)
                DeleteTrxHandles(CurrentInterface, TransHandle);

            HandleErrorCode(retcode);

            Exit:
            HandleErrorCode(retcode);


        }

        private void btnBankaListe_Click(object sender, EventArgs e)
        {
            BankaListesiAl();
        }

        private void btnYemekCekleri_Click(object sender, EventArgs e)
        {
            YemekCekiListesiAl();
        }

        private void BankaListesiAl()
        {
            if (m_tvEcho.Nodes.Count < 1)
            {
                MessageBox.Show("Aktarım Yapmadan Once Eslestirme Yapılması Gerekir");
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
                MessageBox.Show("KrediKartı Odeme Uygulaması Bulunamadı", "KrediKarti Kontrol", MessageBoxButtons.OK);
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

        private void YemekCekiListesiAl()
        {
            if (m_tvEcho.Nodes.Count < 1)
            {
                MessageBox.Show("Aktarım Yapmadan Once Eslestirme Yapılması Gerekir");
                return;
            }

            listBox2.Items.Clear();
            byte NumberOfTotalRecord = 24;
            byte NumberOfTotalRecordReceived = 0;

            ST_PAYMENT_APPLICATION_INFO[] StPaymentApplicationInfo = new ST_PAYMENT_APPLICATION_INFO[24];
            for (int i = 0; i < StPaymentApplicationInfo.Length; i++)
            {
                StPaymentApplicationInfo[i] = new ST_PAYMENT_APPLICATION_INFO();
            }

            uint retcode = Json_GMPSmartDLL.FP3_GetVasApplicationInfo(CurrentInterface, ref NumberOfTotalRecord, ref NumberOfTotalRecordReceived, ref StPaymentApplicationInfo, (ushort)EVasType.TLV_OKC_ASSIST_VAS_TYPE_YEMEKCEKI);

            if (retcode != Defines.TRAN_RESULT_OK)
                HandleErrorCode(retcode);
            else if (NumberOfTotalRecordReceived == 0)
                MessageBox.Show("ÖKC Üzerinde Yemek Ceki Ödeme Uygulanaması Bulunamadı", "HATA", MessageBoxButtons.OK);
            else
            {
                /*
                PaymentAppForm paf = new PaymentAppForm(NumberOfTotalRecordReceived, StPaymentApplicationInfo);
                DialogResult dr = paf.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK)
                    return;
                */

                ST_PAYMENT_APPLICATION_INFO[] stPaymentApplicationInfo2;

                stPaymentApplicationInfo2 = new ST_PAYMENT_APPLICATION_INFO[24];
                Array.Copy(StPaymentApplicationInfo, stPaymentApplicationInfo2, StPaymentApplicationInfo.Length);
                for (int i = 0; i < NumberOfTotalRecordReceived; i++)
                {

                    string str = "";
                    str += GMP_Tools.GetStringFromBytes(StPaymentApplicationInfo[i].name) +
                        " [" + StPaymentApplicationInfo[i].u16BKMId.ToString() + "] " +
                        " [" + StPaymentApplicationInfo[i].u16AppId.ToString("X2") + "] " +
                        " [" + StPaymentApplicationInfo[i].Status.ToString() + "] " +
                        " [" + StPaymentApplicationInfo[i].Priority.ToString() + "]";//+
                    //" [" + getAppTypeName(stPaymentApplicationInfo[i].AppType) + "]";

                    listBox2.Items.Add(str);
                }


            }

        }

        private void FisIptal()
        {
            OnBnClickedButtonVoidAll();
            ClearTransactionUniqueId(CurrentInterface);
        }

        private void FaturaKes(FaturaBilgi fat)
        {
            ST_TICKET m_stTicket = new ST_TICKET();
            ST_INVIOCE_INFO stInvoiceInfo = new ST_INVIOCE_INFO();

            UInt64 activeFlags = 0;

            stInvoiceInfo.source = fat.Tipi;//(byte)m_cmbInvoiceType.SelectedIndex;
            stInvoiceInfo.amount = (ulong)fat.Tutar; //m_txtInvoiceAmount.Text == "" ? 0 : Convert.ToUInt64(m_txtInvoiceAmount.Text);
            stInvoiceInfo.currency = 949;
            /*
            if (m_comboBoxCurrency.Text != "")
                stInvoiceInfo.currency = Convert.ToUInt16(m_comboBoxCurrency.Text.Substring(0, 3));
            */
            stInvoiceInfo.no = new byte[25];
            //            ConvertAscToBcdArray(m_txtInvoiceNo.Text, ref stInvoiceInfo.no, stInvoiceInfo.no.Length);
            ConvertAscToBcdArray(fat.No, ref stInvoiceInfo.no, stInvoiceInfo.no.Length);
            stInvoiceInfo.tck_no = new byte[12];
            //            ConvertAscToBcdArray(m_txtTCK_No.Text, ref stInvoiceInfo.tck_no, stInvoiceInfo.tck_no.Length);
            ConvertAscToBcdArray(fat.Tckno, ref stInvoiceInfo.tck_no, stInvoiceInfo.tck_no.Length);
            stInvoiceInfo.vk_no = new byte[12];
            //            ConvertAscToBcdArray(m_txtVKN.Text, ref stInvoiceInfo.vk_no, stInvoiceInfo.vk_no.Length);
            ConvertAscToBcdArray(fat.Vergino, ref stInvoiceInfo.vk_no, stInvoiceInfo.vk_no.Length);

            stInvoiceInfo.date = new byte[3];
            //            string dateStr = m_dateInvoiceDate.Value.Day.ToString().PadLeft(2, '0') + m_dateInvoiceDate.Value.Month.ToString().PadLeft(2, '0') + m_dateInvoiceDate.Value.Year.ToString().Substring(2, 2).PadLeft(2, '0');
            string dateStr = fat.Tarih.Day.ToString().PadLeft(2, '0') + fat.Tarih.Month.ToString().PadLeft(2, '0') + fat.Tarih.Year.ToString().Substring(2, 2).PadLeft(2, '0');

            ConvertStringToHexArray(dateStr, ref stInvoiceInfo.date, 3);
            Array.Reverse(stInvoiceInfo.date);

            //if (m_chcIrsaliye.Checked)
            if (fat.Irsaliye)
                stInvoiceInfo.flag |= (uint)EInvoiceFlags.INVOICE_FLAG_IRSALIYE;

            if (BatchMode)
            {
                byte[] buffer = new byte[1024];
                int bufferLen = 0;

                bufferLen = Json_GMPSmartDLL.prepare_SetInvoice(buffer, buffer.Length, ref stInvoiceInfo);
                AddIntoCommandBatch("prepare_SetInvoice", Defines.GMP3_FISCAL_PRINTER_MODE_REQ, buffer, bufferLen);

                Array.Clear(buffer, 0, buffer.Length);
                bufferLen = 0;
                bufferLen = GMPSmartDLL.prepare_TicketHeader(buffer, buffer.Length, TTicketType.TInvoice);
                AddIntoCommandBatch("prepare_TicketHeader", Defines.GMP3_FISCAL_PRINTER_MODE_REQ, buffer, bufferLen);

                //tabControl1.SelectedTab = tabPage6;
            }
            else
            {
                UInt32 retcode = 0;
                if (GetTransactionHandle(CurrentInterface) == 0)
                {
                    UInt64 TransactionHandle = 0;
                    retcode = GMPSmartDLL.FP3_Start(CurrentInterface, ref TransactionHandle, isBackground, GetUniqueIdByInterface(CurrentInterface), 24, null, 0, null, 0, Defines.TIMEOUT_DEFAULT);
                    AddTrxHandles(CurrentInterface, TransactionHandle, isBackground);

                    if (retcode != Defines.TRAN_RESULT_OK)
                        HandleErrorCode(retcode);

                    retcode = GMPSmartDLL.FP3_OptionFlags(CurrentInterface, GetTransactionHandle(CurrentInterface), ref activeFlags, (Defines.GMP3_OPTION_ECHO_PRINTER | Defines.GMP3_OPTION_ECHO_ITEM_DETAILS | Defines.GMP3_OPTION_ECHO_PAYMENT_DETAILS), 0x00000000, Defines.TIMEOUT_DEFAULT);
                    if (retcode != Defines.TRAN_RESULT_OK)
                        HandleErrorCode(retcode);
                }

                retcode = Json_GMPSmartDLL.FP3_SetInvoice(CurrentInterface, GetTransactionHandle(CurrentInterface), ref stInvoiceInfo, ref m_stTicket, Defines.TIMEOUT_DEFAULT);
                if (retcode != 0)
                {
                    HandleErrorCode(retcode);
                    return;
                }

                retcode = GMPSmartDLL.FP3_TicketHeader(CurrentInterface, GetTransactionHandle(CurrentInterface), TTicketType.TInvoice, Defines.TIMEOUT_DEFAULT);
                if ((retcode != Defines.TRAN_RESULT_OK) && (retcode != Defines.APP_ERR_TICKET_HEADER_ALREADY_PRINTED))
                {
                    HandleErrorCode(retcode);
                    return;
                }

                HandleErrorCode(retcode);
                DisplayTransaction(m_stTicket, false);
            }

        }

        private void rdRmos_CheckedChanged(object sender, EventArgs e)
        {
            if (rdRmos.Checked)
                groupUltimate.Visible = true;
            else
                groupUltimate.Visible = false;

        }

        private void rdModul_CheckedChanged(object sender, EventArgs e)
        {
            if (rdRmos.Checked)
                groupUltimate.Visible = true;
            else
                groupUltimate.Visible = false;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            DialogResult c = MessageBox.Show("Cıkmak İçin Lütfen Parolayı giriniz..!", "Uyarı", MessageBoxButtons.YesNo);
            if (c == DialogResult.Yes)
            {
                IngenicoPass a = new IngenicoPass();
                a.ShowDialog();

                if (a.Durum == true)
                {
                    DinlemeBitir();
                    this.Close();
                }
                else
                {
                    return;
                }           
            }
            else
            {
                this.Hide();
            }
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult c = MessageBox.Show("Cıkmak İçin Lütfen Parolayı giriniz..!", "Uyarı", MessageBoxButtons.YesNo);
            if (c == DialogResult.Yes)
            {
                IngenicoPass a = new IngenicoPass();
                a.ShowDialog();

                if (a.Durum == true)
                {
                    DinlemeBitir();
                    this.Close();
                }
                else
                {
                    return;
                }
            }
            else
            {
                this.Hide();
            }
        }

        private void btnKK_Click(object sender, EventArgs e)
        {
            OdemeKK(Convert.ToInt32(m_txtInputData.Text), 0);
        }

        private void btnYemekCeki_Click(object sender, EventArgs e)
        {
            OdemeYemekCeki(Convert.ToInt32(m_txtInputData.Text), 0);

        }

        private void btnFatura_Click(object sender, EventArgs e)
        {
            FaturaBilgi fat = new FaturaBilgi();
            fat.No = "11223344";
            fat.Tarih = DateTime.Now;
            fat.Tckno = "42179019292";
            fat.Vergino = "1652893987";
            fat.Tutar = 100;
            fat.Tipi = 0;
            fat.Irsaliye = false;

            FaturaKes(fat);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                taskbar.Visible = true;
                taskbar.ShowBalloonTip(3000);
            }
        }

        private void taskbar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            taskbar.Visible = false;
        }



    }
}
