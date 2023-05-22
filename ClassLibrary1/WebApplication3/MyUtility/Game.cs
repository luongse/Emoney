using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtility
{
    public partial class EnumManager
    {
        /// <summary>
        /// TanPVD:2
        /// GameID của table Game trong db Web
        /// </summary>
        public enum ENM_GameID : int
        {
            /// <summary>
            /// 1 : Ca rô
            /// </summary>
            Caro = 1,

            /// <summary>
            /// 2 : Cờ tướng
            /// </summary>
            CoTuong = 2,

            /// <summary>
            /// 3 : Cờ vua
            /// </summary>
            CoVua = 3,

            /// <summary>
            /// 4 : Domino
            /// </summary>
            Domino = 4,

            /// <summary>
            /// 5 : Catte , Cá nước
            /// </summary>
            Catte = 5,

            /// <summary>
            /// 6 : co ca ngua
            /// </summary>
            CoCaNgua = 6,

            /// <summary>
            /// 7 : Tiến lên MN, Bắt heo 2
            /// </summary>
            TienLen_MienNam = 7,

            /// <summary>
            /// 8 : Binh tính chi, Sập hầm
            /// </summary>
            BinhTinhChi = 8,

            /// <summary>
            /// 9 : Cờ gánh
            /// </summary>
            CoGanh = 9,

            /// <summary>
            /// 12 : Tiến lên MB, Bắt heo 1
            /// </summary>
            TienLen_MienBac = 12,

            /// <summary>
            /// 15 : Ù 9 lá, Ù ăn gà
            /// </summary>
            U9La = 15,

            /// <summary>
            /// 16 : Xì tố, Lật tẩy
            /// </summary>
            XiTo = 16,

            /// <summary>
            /// 17 : Lô tô
            /// </summary>
            LoTo = 17,

            /// <summary>
            /// 18 : Xì dách, 2 cây
            /// </summary>
            XiDach = 18,

            /// <summary>
            /// 19 : Săn quà
            /// </summary>
            SanQua = 19,

            /// <summary>
            /// 20 : Pocker
            /// </summary>
            Pocker = 20,

            /// <summary>
            /// 21 : Black Jack, jack đen
            /// </summary>
            BlackJack = 21,

            /// <summary>
            /// 22 : Đào Vàng
            /// </summary>
            DaoVang = 22,

            /// <summary>
            /// 23 : Đánh Chắn, Gà nhái
            /// </summary>
            DanhChan = 23,

            /// <summary>
            /// 25 : Phỏm 8 lá
            /// </summary>
            Phom8La = 25,

            /// <summary>
            /// 26 : Game đua ngựa
            /// </summary>
            DuaNgua = 26,

            /// <summary>
            /// 28 : Ba lá
            /// </summary>
            BaLa = 28,

            /// <summary>
            /// 29: xi to 3.0
            /// </summary>
            Xito30 = 29,

            /// <summary>
            /// 30: U 9 la 3.0
            /// </summary>
            U9la30 = 30,

            /// <summary>
            /// 31: TienLenMN 3.0
            /// </summary>
            TienLenMN30 = 31,

            /// <summary>
            /// 33: Danh Chan 3.0
            /// </summary>
            DanhChan30 = 33,

            /// <summary>
            /// 34 : Binh tinh chi 3.0
            /// </summary>
            Binh30 = 34,

            /// <summary>
            /// 35: Black jack 3.0
            /// </summary>
            BlackJack30 = 35,

            /// <summary>
            /// 36: Xi Dzach 3.0
            /// </summary>
            XiDzach30 = 36,

            /// <summary>
            /// 41: xi to 4.0
            /// </summary>
            Xito40 = 42,

            /// <summary>
            /// 42: U 9 la 4.0
            /// </summary>
            U9la40 = 41,

            /// <summary>
            /// 40: TienLenMN 4.0
            /// </summary>
            TienLenMN40 = 40,

            /// <summary>
            /// 43: Poker 4.0
            /// </summary>
            Poker40 = 43,

            /// <summary>
            /// 44: Danh Chan 4.0
            /// </summary>
            DanhChan40 = 44,

            /// <summary>
            /// 45: Tai Xiu
            /// </summary>
            TaiXiu = 45,

            /// <summary>
            /// 200 : game ca`o to'
            /// </summary>
            Lieng = 200,

            /// <summary>
            /// 201 : game sa^m lo^'c
            /// </summary>
            SamLoc = 201,

            /// <summary>
            /// 202 : game co` ca' ngu.a
            /// </summary>
            CaNgua = 202,

            /// <summary>
            /// 203 : Game Bar (Bass)
            /// </summary>
            Bar = 203,

            /// <summary>
            /// 204 : Game Keno
            /// </summary>
            Keno = 204,

            /// <summary>
            /// 205 : Game Quay Xiềng, Quay Số
            /// </summary>
            QuaySo = 205,

            /// <summary>
            /// 206 : Game Cờ Tướng 4.0
            /// </summary>
            CoTuong40 = 206,

            /// <summary>
            /// 207 : Game Cờ úp
            /// </summary>
            CoUp = 207,

            /// <summary>
            /// 208 : game lucky online
            /// </summary>
            LuckyOnline = 208,

            /// <summary>
            /// 209 : game xoc dia
            /// </summary>
            XocDia = 209,

            /// <summary>
            /// 210: Oan tu ti
            /// </summary>
            OanTuTi = 210,

            /// <summary>
            /// 250 : game danh chan 4 beta
            /// </summary>
            DanhChan42 = 250,

            /// <summary>
            /// 251 : game tien len mien nam 4.1
            /// </summary>
            TienLenMN41 = 251,

            /// <summary>
            /// 210: game oan tu xi
            /// </summary>
            OanTuXi = 210
        }

        public static int ToInt(ENM_GameID Value)
        {
            return (int)Value;
        }

        public static ENM_GameID ToEnum_GameID(int Value)
        {
            try
            {
                return (ENM_GameID)Enum.Parse(typeof(ENM_GameID), Value.ToString());
            }
            catch (Exception exp)
            {
                throw new Exception("Giá trị của enum game id không đúng : " + Value.ToString(), exp);

            }
        }
    }
}
