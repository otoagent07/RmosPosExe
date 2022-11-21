using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraEditors.Controls;

namespace Pos.Class
{
    public class Localize
    {
        public static void ApplicationLanguage(string lng)
        {
            if (lng == "tr")
            {
                GridLocalizer.Active = new TurkishGridLocalizer();
                Localizer.Active = new TurkishEditorsLocalizer();
            }
            if (lng == "en")
            {
                GridLocalizer.Active = null;
                Localizer.Active = null;
            }
        }
    }

    public class TurkishGridLocalizer : GridLocalizer
    {
        public override string Language { get { return "Turkish"; } }
        public override string GetLocalizedString(GridStringId id)
        {
            string ret = "";
            switch (id)
            {
                // ...
                case GridStringId.MenuColumnSortAscending: return "Artan Sıralama";
                case GridStringId.MenuColumnSortDescending: return "Azalan Sıralama";
                case GridStringId.MenuColumnClearSorting: return "Sıralamayı İptal Et";
                case GridStringId.GridGroupPanelText: return "Gruplamak İstediğiniz Kolonu Sürükleyip Buraya Bırakınız...";
                case GridStringId.MenuColumnGroup: return "Bu Kolona Göre Grupla";
                case GridStringId.MenuColumnUnGroup: return "Gruplamayı Kaldır";
                case GridStringId.MenuGroupPanelShow: return "Gruplamayı Göster";
                case GridStringId.MenuGroupPanelHide: return "Gruplamayı Kaldır";
                case GridStringId.MenuColumnRemoveColumn: return "Kolon Gizle";
                case GridStringId.MenuColumnFindFilterShow: return "Arama Paneli";
                case GridStringId.MenuColumnFindFilterHide: return "Arama Paneli Gizle";
                case GridStringId.MenuColumnAutoFilterRowShow: return "Kolon Filtreleme";
                case GridStringId.MenuColumnAutoFilterRowHide: return "Kolon Filtreleme Gizle";
                case GridStringId.MenuColumnGroupBox: return "Kolon Gruplama";
                case GridStringId.MenuColumnColumnCustomization: return "Kolon Seç";
                case GridStringId.MenuColumnBestFit: return "En Uygun Genişlik";
                case GridStringId.MenuColumnBestFitAllColumns: return "Tüm Kolonlar Uygun Genişlik";
                case GridStringId.CustomizationCaption: return "Kolon Seçici";
                case GridStringId.MenuColumnFilterEditor: return "Filtreleme Editörü";
                case GridStringId.FilterBuilderCaption: return "Filtreleme";
                case GridStringId.FilterBuilderApplyButton: return "Uygula";
                case GridStringId.FilterBuilderCancelButton: return "İptal";
                case GridStringId.FilterBuilderOkButton: return "Tamam";
                case GridStringId.MenuGroupPanelClearGrouping: return "Gruplamayı İptal Et";
                case GridStringId.MenuGroupPanelFullCollapse: return "Gruplanmış Kayıtları Gizle";
                case GridStringId.MenuGroupPanelFullExpand: return "Gruplanmış Kayıtları Göster";
                case GridStringId.PopupFilterBlanks: return "(Boş Olanları Getir)";
                case GridStringId.PopupFilterAll: return "(Tümünü Getir)";
                case GridStringId.MenuColumnClearFilter : return "Filtrelemeyi İptal Et";
                case GridStringId.PopupFilterNonBlanks: return "(Boş Olmayanları Getir)";
                case GridStringId.PopupFilterCustom: return "(Manuel Filtreleme)";
                case GridStringId.CustomFilterDialogCaption: return "Filtenizi Oluşturunuz";
                case GridStringId.CustomFilterDialogCancelButton: return "İptal";
                case GridStringId.CustomFilterDialogOkButton: return "Tamam";
                case GridStringId.CustomFilterDialogFormCaption: return "Filtreleme";
                //case GridStringId.CustomFilterDialogConditionEQU: return "Eşitir";
                //case GridStringId.CustomFilterDialogConditionNEQ: return "Eşit Değildir";
                //case GridStringId.CustomFilterDialogConditionNonBlanks: return "Boş Olmayan";
                //case GridStringId.CustomFilterDialogConditionBlanks: return "Boş Olan";
                case GridStringId.GridNewRowText: return "Yeni Kayıt Kklemek İçin Tıklayınız...";
                case GridStringId.WindowErrorCaption: return "Hata";
                case GridStringId.ColumnViewExceptionMessage: return "Tekrar Denemek İstiyor musunuz ?";
                case GridStringId.MenuFooterAverage: return "Ortalama";

                case GridStringId.MenuFooterCount: return "Say";
                case GridStringId.FindControlClearButton: return "Temizle";
                case GridStringId.FindControlFindButton: return "Bul";

                case GridStringId.GroupSummaryEditorFormCaption: return "Bul";

                case GridStringId.MenuFooterMax: return "En Büyük";
                case GridStringId.MenuFooterMin: return "En Küçük";
                case GridStringId.MenuFooterNone: return "Hiç Biri";
                case GridStringId.MenuFooterSum: return "Toplam";

                //// ...
                //default:
                //    ret = id.ToString();
                //    break;
            }
            return ret;
        }
    }

    public class TurkishEditorsLocalizer : Localizer
    {
        public override string Language { get { return "Turkish"; } }
        public override string GetLocalizedString(StringId id)
        {
            string ret = "";
            switch (id)
            {
                // ...
                case StringId.NavigatorTextStringFormat: return "Kayıt {0} / {1}";
                case StringId.PictureEditMenuCut: return "Kes";
                case StringId.PictureEditMenuCopy: return "Kopyala";
                case StringId.PictureEditMenuPaste: return "Yapıştır";
                case StringId.PictureEditMenuDelete: return "Sil";
                case StringId.PictureEditMenuLoad: return "Laden";
                case StringId.PictureEditMenuSave: return "Kaydet";
                case StringId.XtraMessageBoxYesButtonText: return "Evet";
                case StringId.XtraMessageBoxNoButtonText: return "Hayır";
                case StringId.XtraMessageBoxAbortButtonText: return "Vazgeç";
                case StringId.XtraMessageBoxCancelButtonText: return "İptal";
                case StringId.XtraMessageBoxIgnoreButtonText: return "Görme";
                case StringId.XtraMessageBoxOkButtonText: return "Tamam";
                case StringId.XtraMessageBoxRetryButtonText: return "Tekrar Dene";
                case StringId.UnknownPictureFormat: return "Bilinmeyen Resim Tipi";
                case StringId.Apply: return "Uygula";
                case StringId.CalcButtonBack: return "Geri";
                case StringId.Cancel: return "İptal";
                case StringId.CaptionError: return "Hata";
                case StringId.DateEditClear: return "Temizle";
                case StringId.DateEditToday: return "Bugün";
                case StringId.FilterClauseEquals: return "Eşittir";
                case StringId.FilterClauseDoesNotEqual: return "Eşit Değildir";
                case StringId.FilterClauseGreater: return "Büyüktür";
                case StringId.FilterClauseGreaterOrEqual: return "Büyük veya Eşit";
                case StringId.FilterClauseIsNotNull: return "Değer İçeren";
                case StringId.FilterClauseIsNull: return "Değer İçermeyen";
                case StringId.FilterClauseLess: return "Küçüktür";
                case StringId.FilterClauseLessOrEqual: return "Küçük Veya Eşit";
                case StringId.FilterClauseLike: return "Benzer";
                case StringId.FilterClauseNoneOf: return "Hiçbiri";
                case StringId.FilterClauseBetween: return "Arasında";
                case StringId.FilterClauseNotBetween: return "Arasında Olmayan";
                case StringId.FilterClauseNotLike: return "Benzemeyen";
                case StringId.FilterMenuClearAll: return "Tümünü Temizle";
                case StringId.FilterShowAll: return "Tümünü Göster";
                case StringId.InvalidValueText: return "Geçersiz Değer";
                case StringId.LookUpEditValueIsNull: return "Seçiniz";
                case StringId.NavigatorAppendButtonHint: return "Yeni Kayıt";
                case StringId.NavigatorCancelEditButtonHint: return "Vazgeç";
                case StringId.NavigatorEditButtonHint: return "Güncelle";
                case StringId.NavigatorEndEditButtonHint: return "Kaydet";
                case StringId.NavigatorFirstButtonHint: return "İlk Kayıt";
                case StringId.NavigatorLastButtonHint: return "Son Kayıt";
                case StringId.NavigatorNextButtonHint: return "İleri";
                case StringId.NavigatorNextPageButtonHint: return "İleri Sayfa";
                case StringId.NavigatorPreviousButtonHint: return "Geri";
                case StringId.NavigatorPreviousPageButtonHint: return "Geri Sayfa";
                case StringId.NavigatorRemoveButtonHint: return "Sil";
                case StringId.None: return "Hiçbiri";
                case StringId.OK: return "Tamam";
                case StringId.NotValidArrayLength: return "Geçersiz Dizi Uzunluğu";
                case StringId.TextEditMenuCopy: return "Kopyala";
                case StringId.TextEditMenuCut: return "Kes";
                case StringId.TextEditMenuDelete: return "Sil";
                case StringId.TextEditMenuPaste: return "Yapıştır";
                case StringId.TextEditMenuSelectAll: return "Tümünü Seç";
                case StringId.TextEditMenuUndo: return "Geri Al";
                /////
                case StringId.FilterOutlookDateText:
                    return "Tümünü Göster|Boş Olanlar|Tarihe Göre Filtre:|Önceki Yıl|Sonraki Yıl|Sonraki Ay|Gelecek Hafta|" +
                          "Sonraki Hafta|Yarın|Bugün|Dün|Önceki Hafta|Geçen Hafta|Bu Ay Başı|Bu Yıl Başı|" +
                          "Gecen Yıl";
                // ...
                default:
                    ret = id.ToString();
                    break;
            }
            return ret;
        }
    }
}
