using System;
using System.Runtime.Serialization;

namespace Ucoin.Resource.Entity
{
    ///<summary>
    ///指定标准的纸张大小。
    ///</summary>
    [Serializable, DataContract]
    public enum PaperSizeType
    {
        ///<summary>
        ///Letter paper (8.5 in.by 11 in.).
        ///</summary>
        [EnumMember]
        Letter = 1,
        ///<summary>
        ///Letter small paper (8.5 in.by 11 in.).
        ///</summary>
        [EnumMember]
        LetterSmall = 2,
        ///<summary>
        ///Tabloid paper (11 in.by 17 in.).
        ///</summary>
        [EnumMember]
        Tabloid = 3,
        ///<summary>
        ///Ledger paper (17 in.by 11 in.).
        ///</summary>
        [EnumMember]
        Ledger = 4,
        ///<summary>
        ///Legal paper (8.5 in.by 14 in.).
        ///</summary>
        [EnumMember]
        Legal = 5,
        ///<summary>
        ///Statement paper (5.5 in.by 8.5 in.).
        ///</summary>
        [EnumMember]
        Statement = 6,
        ///<summary>
        ///Executive paper (7.25 in.by 10.5 in.).
        ///</summary>
        [EnumMember]
        Executive = 7,
        ///<summary>
        ///A3 纸（297 毫米 × 420 毫米）。
        ///</summary>
        [EnumMember]
        A3 = 8,
        ///<summary>
        ///A4 纸（210 毫米 × 297 毫米）。
        ///</summary>
        [EnumMember]
        A4 = 9,
        ///<summary>
        ///A4 small 纸（210 毫米 × 297 毫米）。
        ///</summary>
        [EnumMember]
        A4Small = 10,
        ///<summary>
        ///A5 纸（148 毫米 × 210 毫米）。
        ///</summary>
        [EnumMember]
        A5 = 11,
        ///<summary>
        ///B4 纸（250 × 353 毫米）。
        ///</summary>
        [EnumMember]
        B4 = 12,
        ///<summary>
        ///B5 纸（176 毫米 × 250 毫米）。
        ///</summary>
        [EnumMember]
        B5 = 13,
        ///<summary>
        ///Folio paper (8.5 in.by 13 in.).
        ///</summary>
        [EnumMember]
        Folio = 14,
        ///<summary>
        ///Quarto 纸（215 毫米 × 275 毫米）。
        ///</summary>
        [EnumMember]
        Quarto = 15,
        ///<summary>
        ///Standard paper (10 in.by 14 in.).
        ///</summary>
        [EnumMember]
        Standard10x14 = 16,
        ///<summary>
        ///Standard paper (11 in.by 17 in.).
        ///</summary>
        [EnumMember]
        Standard11x17 = 17,
        ///<summary>
        ///Note paper (8.5 in.by 11 in.).
        ///</summary>
        [EnumMember]
        Note = 18,
        ///<summary>
        ///#9 envelope (3.875 in.by 8.875 in.).
        ///</summary>
        [EnumMember]
        Number9Envelope = 19,
        ///<summary>
        ///#10 envelope (4.125 in.by 9.5 in.).
        ///</summary>
        [EnumMember]
        Number10Envelope = 20,
        ///<summary>
        ///#11 envelope (4.5 in.by 10.375 in.).
        ///</summary>
        [EnumMember]
        Number11Envelope = 21,
        ///<summary>
        ///#12 envelope (4.75 in.by 11 in.).
        ///</summary>
        [EnumMember]
        Number12Envelope = 22,
        ///<summary>
        ///#14 envelope (5 in.by 11.5 in.).
        ///</summary>
        [EnumMember]
        Number14Envelope = 23,
        ///<summary>
        ///C paper (17 in.by 22 in.).
        ///</summary>
        [EnumMember]
        CSheet = 24,
        ///<summary>
        ///D paper (22 in.by 34 in.).
        ///</summary>
        [EnumMember]
        DSheet = 25,
        ///<summary>
        ///E paper (34 in.by 44 in.).
        ///</summary>
        [EnumMember]
        ESheet = 26,
        ///<summary>
        ///DL 信封（110 毫米 × 220 毫米）。
        ///</summary>
        [EnumMember]
        DLEnvelope = 27,
        ///<summary>
        ///C5 信封（162 毫米 × 229 毫米）。
        ///</summary>
        [EnumMember]
        C5Envelope = 28,
        ///<summary>
        ///C3 信封（324 毫米 × 458 毫米）。
        ///</summary>
        [EnumMember]
        C3Envelope = 29,
        ///<summary>
        ///C4 信封（229 毫米 × 324 毫米）。
        ///</summary>
        [EnumMember]
        C4Envelope = 30,
        ///<summary>
        ///C6 信封（114 毫米 × 162 毫米）。
        ///</summary>
        [EnumMember]
        C6Envelope = 31,
        ///<summary>
        ///C65 信封（114 毫米 × 229 毫米）。
        ///</summary>
        [EnumMember]
        C65Envelope = 32,
        ///<summary>
        ///B4 信封（250 × 353 毫米）。
        ///</summary>
        [EnumMember]
        B4Envelope = 33,
        ///<summary>
        ///B5 信封（176 毫米 × 250 毫米）。
        ///</summary>
        [EnumMember]
        B5Envelope = 34,
        ///<summary>
        ///B6 信封（176 毫米 × 125 毫米）。
        ///</summary>
        [EnumMember]
        B6Envelope = 35,
        ///<summary>
        ///Italy envelope（110 毫米 × 230 毫米）。
        ///</summary>
        [EnumMember]
        ItalyEnvelope = 36,
        ///<summary>
        ///Monarch envelope (3.875 in.by 7.5 in.).
        ///</summary>
        [EnumMember]
        MonarchEnvelope = 37,
        ///<summary>
        ///6 3/4 envelope (3.625 in.by 6.5 in.).
        ///</summary>
        [EnumMember]
        PersonalEnvelope = 38,
        ///<summary>
        ///US standard fanfold (14.875 in.by 11 in.).
        ///</summary>
        [EnumMember]
        USStandardFanfold = 39,
        ///<summary>
        ///German standard fanfold (8.5 in.by 12 in.).
        ///</summary>
        [EnumMember]
        GermanStandardFanfold = 40,
        ///<summary>
        ///German legal fanfold (8.5 in.by 13 in.).
        ///</summary>
        [EnumMember]
        GermanLegalFanfold = 41,
        ///<summary>
        ///ISO B4（250 毫米 × 353 毫米）。
        ///</summary>
        [EnumMember]
        IsoB4 = 42,
        ///<summary>
        ///Japanese postcard（100 毫米 × 148 毫米）。
        ///</summary>
        [EnumMember]
        JapanesePostcard = 43,
        ///<summary>
        ///Standard paper (9 in.by 11 in.).
        ///</summary>
        [EnumMember]
        Standard9x11 = 44,
        ///<summary>
        ///Standard paper (10 in.by 11 in.).
        ///</summary>
        [EnumMember]
        Standard10x11 = 45,
        ///<summary>
        ///Standard paper (15 in.by 11 in.).
        ///</summary>
        [EnumMember]
        Standard15x11 = 46,
        ///<summary>
        ///邀请函信封（220 毫米 × 220 毫米）。
        ///</summary>
        [EnumMember]
        InviteEnvelope = 47,
        ///<summary>
        ///Letter extra paper (9.275 in.by 12 in.).该值特定于 PostScript 驱动程序，仅供 Linotronic
        ///打印机使用以节省纸张。
        ///</summary>
        [EnumMember]
        LetterExtra = 50,
        ///<summary>
        ///Legal extra paper (9.275 in.by 15 in.).该值特定于 PostScript 驱动程序，仅供 Linotronic
        ///打印机使用以节省纸张。
        ///</summary>
        [EnumMember]
        LegalExtra = 51,
        ///<summary>
        ///Tabloid extra paper (11.69 in.by 18 in.).该值特定于 PostScript 驱动程序，仅供 Linotronic
        ///打印机使用以节省纸张。
        ///</summary>
        [EnumMember]
        TabloidExtra = 52,
        ///<summary>
        ///A4 extra 纸（236 毫米 × 322 毫米）。该值是针对 PostScript 驱动程序的，仅供 Linotronic 打印机使用以节省纸张。
        ///</summary>
        [EnumMember]
        A4Extra = 53,
        ///<summary>
        ///Letter transverse paper (8.275 in.by 11 in.).
        ///</summary>
        [EnumMember]
        LetterTransverse = 54,
        ///<summary>
        ///A4 transverse 纸（210 毫米 × 297 毫米）。
        ///</summary>
        [EnumMember]
        A4Transverse = 55,
        ///<summary>
        ///Letter extra transverse paper (9.275 in.by 12 in.).
        ///</summary>
        [EnumMember]
        LetterExtraTransverse = 56,
        ///<summary>
        ///SuperA/SuperA/A4 纸（227 毫米 × 356 毫米）。
        ///</summary>
        [EnumMember]
        APlus = 57,
        ///<summary>
        ///SuperB/SuperB/A3 纸（305 毫米 × 487 毫米）。
        ///</summary>
        [EnumMember]
        BPlus = 58,
        ///<summary>
        ///Letter plus paper (8.5 in.by 12.69 in.).
        ///</summary>
        [EnumMember]
        LetterPlus = 59,
        ///<summary>
        ///A4 plus 纸（210 毫米 × 330 毫米）。
        ///</summary>
        [EnumMember]
        A4Plus = 60,
        ///<summary>
        ///A5 transverse 纸（148 毫米 × 210 毫米）。
        ///</summary>
        [EnumMember]
        A5Transverse = 61,
        ///<summary>
        ///JIS B5 transverse 纸（182 毫米 × 257 毫米）。
        ///</summary>
        [EnumMember]
        B5Transverse = 62,
        ///<summary>
        ///A3 extra 纸（322 毫米 × 445 毫米）。
        ///</summary>
        [EnumMember]
        A3Extra = 63,
        ///<summary>
        ///A5 extra 纸（174 毫米 × 235 毫米）。
        ///</summary>
        [EnumMember]
        A5Extra = 64,
        ///<summary>
        ///ISO B5 extra 纸（201 毫米 × 276 毫米）。
        ///</summary>
        [EnumMember]
        B5Extra = 65,
        ///<summary>
        ///A2 纸（420 毫米 × 594 毫米）。
        ///</summary>
        [EnumMember]
        A2 = 66,
        ///<summary>
        ///A3 transverse 纸（297 毫米 × 420 毫米）。
        ///</summary>
        [EnumMember]
        A3Transverse = 67,
        ///<summary>
        ///A3 extra transverse 纸（322 毫米 × 445 毫米）。
        ///</summary>
        [EnumMember]
        A3ExtraTransverse = 68,
        ///<summary>
        ///Japanese double postcard（200 毫米 × 148 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        JapaneseDoublePostcard = 69,
        ///<summary>
        ///A6 纸（105 毫米 × 148 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        A6 = 70,
        ///<summary>
        ///Japanese Kaku #2 envelope。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        JapaneseEnvelopeKakuNumber2 = 71,
        ///<summary>
        ///Japanese Kaku #3 envelope。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        JapaneseEnvelopeKakuNumber3 = 72,
        ///<summary>
        ///Japanese Chou #3 envelope。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        JapaneseEnvelopeChouNumber3 = 73,
        ///<summary>
        ///Japanese Chou #4 envelope。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        JapaneseEnvelopeChouNumber4 = 74,
        ///<summary>
        ///Letter rotated paper (11 in.by 8.5 in.).
        ///</summary>
        [EnumMember]
        LetterRotated = 75,
        ///<summary>
        ///A3 rotated 纸（420 毫米 × 297 毫米）。
        ///</summary>
        [EnumMember]
        A3Rotated = 76,
        ///<summary>
        ///A4 rotated 纸（297 毫米 × 210 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        A4Rotated = 77,
        ///<summary>
        ///A5 rotated 纸（210 毫米 × 148 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        A5Rotated = 78,
        ///<summary>
        ///JIS B4 rotated paper (364 mm by 257 mm).需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        B4JisRotated = 79,
        ///<summary>
        ///JIS B5 rotated 纸（257 毫米 × 182 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        B5JisRotated = 80,
        ///<summary>
        ///Japanese rotated postcard（148 毫米 × 100 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        JapanesePostcardRotated = 81,
        ///<summary>
        ///Japanese rotated double postcard（148 毫米 × 200 毫米）。需要 Windows 98、Windows NT
        ///4.0 或更高版本。
        ///</summary>
        [EnumMember]
        JapaneseDoublePostcardRotated = 82,
        ///<summary>
        ///A6 rotated 纸（148 毫米 × 105 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        A6Rotated = 83,
        ///<summary>
        ///Japanese rotated Kaku #2 envelope。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        JapaneseEnvelopeKakuNumber2Rotated = 84,
        ///<summary>
        ///Japanese rotated Kaku #3 envelope。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        JapaneseEnvelopeKakuNumber3Rotated = 85,
        ///<summary>
        ///Japanese rotated Chou #3 envelope。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        JapaneseEnvelopeChouNumber3Rotated = 86,
        ///<summary>
        ///Japanese rotated Chou #4 envelope。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        JapaneseEnvelopeChouNumber4Rotated = 87,
        ///<summary>
        ///JIS B6 纸（128 毫米 × 182 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        B6Jis = 88,
        ///<summary>
        ///JIS B6 rotated 纸 (182 × 128 毫米)。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        B6JisRotated = 89,
        ///<summary>
        ///Standard paper (12 in.by 11 in.).需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        Standard12x11 = 90,
        ///<summary>
        ///Japanese You #4 envelope。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        JapaneseEnvelopeYouNumber4 = 91,
        ///<summary>
        ///Japanese You #4 rotated envelope。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        JapaneseEnvelopeYouNumber4Rotated = 92,
        ///<summary>
        ///PRC 16K 纸（146 × 215 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        Prc16K = 93,
        ///<summary>
        ///PRC 32K 纸（97 × 151 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        Prc32K = 94,
        ///<summary>
        ///PRC 32K(Big) 纸（97 × 151 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        Prc32KBig = 95,
        ///<summary>
        ///PRC #1 envelope（102 × 165 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber1 = 96,
        ///<summary>
        ///PRC #2 envelope（102 × 176 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber2 = 97,
        ///<summary>
        ///PRC #3 envelope（125 × 176 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber3 = 98,
        ///<summary>
        ///PRC #4 envelope（110 × 208 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber4 = 99,
        ///<summary>
        ///PRC #5 envelope（110 × 220 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber5 = 100,
        ///<summary>
        ///PRC #6 envelope（120 × 230 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber6 = 101,
        ///<summary>
        ///PRC #7 envelope（160 × 230 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber7 = 102,
        ///<summary>
        ///PRC #8 envelope（120 × 309 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber8 = 103,
        ///<summary>
        ///PRC #9 envelope（229 × 324 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber9 = 104,
        ///<summary>
        ///PRC #10 envelope（324 × 458 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber10 = 105,
        ///<summary>
        ///PRC 16K rotated 纸（146 × 215 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        Prc16KRotated = 106,
        ///<summary>
        ///PRC 32K rotated 纸（97 × 151 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        Prc32KRotated = 107,
        ///<summary>
        ///PRC 32K rotated 纸（97 × 151 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        Prc32KBigRotated = 108,
        ///<summary>
        ///PRC #1 rotated envelope（165 × 102 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber1Rotated = 109,
        ///<summary>
        ///PRC #2 rotated envelope（176 × 102 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber2Rotated = 110,
        ///<summary>
        ///PRC #3 rotated envelope（176 × 125 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber3Rotated = 111,
        ///<summary>
        ///PRC #4 rotated envelope（208 × 110 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber4Rotated = 112,
        ///<summary>
        ///PRC #5 rotated envelope（220 × 110 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber5Rotated = 113,
        ///<summary>
        ///PRC #6 rotated envelope（230 × 120 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber6Rotated = 114,
        ///<summary>
        ///PRC #7 rotated envelope（230 × 160 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber7Rotated = 115,
        ///<summary>
        ///PRC #8 rotated envelope（309 × 120 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber8Rotated = 116,
        ///<summary>
        ///PRC #9 rotated envelope（229 × 324 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber9Rotated = 117,
        ///<summary>
        ///PRC #10 rotated envelope（458 × 324 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
        ///</summary>
        [EnumMember]
        PrcEnvelopeNumber10Rotated = 118,
    }
}
