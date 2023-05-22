/**********************************************************************
 * Author: PhatVT
 * DateCreate: 11-20-2014 
 * Description: Define network common static function 
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 * 
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.ComponentModel;
using System.Web;
namespace MyUtility
{
    public class NetworkCommon
    {
        public enum HttpRequestEnum
        {
            [Description("GET")]
            Get,

            [Description("POST")]
            Post,

            [Description("DELETE")]
            Delete
        }

        public static bool IsForeign(string ip)
        {
            if (ip == "127.0.0.1")
            {
                return false;
            }
            return IsForeign(IPtoLong(ip));
        }

        public static ulong IPtoLong(string ipAddress)
        {
            System.Net.IPAddress ip;
            if (System.Net.IPAddress.TryParse(ipAddress, out ip))
                return (ulong)((ip.GetAddressBytes()[0] << 24) | (ip.GetAddressBytes()[1] << 16) | (ip.GetAddressBytes()[2] << 8) | ip.GetAddressBytes()[3]);
            else return 0;
        }

        /// <summary>
        /// Kiểm tra xem Ip có phải ở nước ngoài không
        /// </summary>
        /// <param name="longIp"></param>
        /// <returns></returns>
        public static bool IsForeign(ulong longIp)
        {
            if (longIp >= 18446744072828100000 && longIp <= 18446744072828100000) { return false; }
            if (longIp >= 18446744072828100000 && longIp <= 18446744072828100000) { return false; }
            if (longIp >= 18446744072828100000 && longIp <= 18446744072828100000) { return false; }
            if (longIp >= 18446744072828100000 && longIp <= 18446744072828100000) { return false; }
            if (longIp >= 1970961408 && longIp <= 1970962430) { return false; }
            if (longIp >= 18446744072828100000 && longIp <= 18446744072828100000) { return false; }
            if (longIp >= 18446744072828100000 && longIp <= 18446744072828100000) { return false; }
            if (longIp >= 18446744072828100000 && longIp <= 18446744072828100000) { return false; }
            if (longIp >= 18446744072828100000 && longIp <= 18446744072828100000) { return false; }
            if (longIp >= 18446744072806600000 && longIp <= 18446744072806600000) { return false; }
            if (longIp >= 710936064 && longIp <= 710936318) { return false; }
            if (longIp >= 710936576 && longIp <= 710936830) { return false; }
            if (longIp >= 18446744072827700000 && longIp <= 18446744072827800000) { return false; }
            if (longIp >= 18446744073120700000 && longIp <= 18446744073120700000) { return false; }
            if (longIp >= 2112487424 && longIp <= 2112618494) { return false; }
            if (longIp >= 1962934272 && longIp <= 1963458558) { return false; }
            if (longIp >= 1934098432 && longIp <= 1934622718) { return false; }
            if (longIp >= 457179136 && longIp <= 458227710) { return false; }
            if (longIp >= 18446744072298100000 && longIp <= 18446744072300200000) { return false; }
            if (longIp >= 1952448512 && longIp <= 1953497086) { return false; }
            if (longIp >= 2111078400 && longIp <= 2111111166) { return false; }
            if (longIp >= 2111176704 && longIp <= 2111193086) { return false; }
            if (longIp >= 18446744072832800000 && longIp <= 18446744072832800000) { return false; }
            if (longIp >= 1969733632 && longIp <= 1969750014) { return false; }
            if (longIp >= 18446744072830900000 && longIp <= 18446744072831000000) { return false; }
            if (longIp >= 18446744072834100000 && longIp <= 18446744072834100000) { return false; }
            if (longIp >= 18446744073130900000 && longIp <= 18446744073131000000) { return false; }
            if (longIp >= 18446744072830800000 && longIp <= 18446744072830800000) { return false; }
            if (longIp >= 18446744073155600000 && longIp <= 18446744073155900000) { return false; }
            if (longIp >= 2064646144 && longIp <= 2065694718) { return false; }
            if (longIp >= 1906311168 && longIp <= 1908408318) { return false; }
            if (longIp >= 245366784 && longIp <= 247463934) { return false; }
            if (longIp >= 249561088 && longIp <= 251658238) { return false; }
            if (longIp >= 18446744073131000000 && longIp <= 18446744073131000000) { return false; }
            if (longIp >= 18446744073131000000 && longIp <= 18446744073131000000) { return false; }
            if (longIp >= 18446744073131000000 && longIp <= 18446744073131000000) { return false; }
            if (longIp >= 18446744073130200000 && longIp <= 18446744073130200000) { return false; }
            if (longIp >= 1953890304 && longIp <= 1953923070) { return false; }
            if (longIp >= 18446744072440500000 && longIp <= 18446744072440600000) { return false; }
            if (longIp >= 1741175808 && longIp <= 1741176830) { return false; }
            if (longIp >= 18446744072833200000 && longIp <= 18446744072833200000) { return false; }
            if (longIp >= 1891958784 && longIp <= 1892024318) { return false; }
            if (longIp >= 453115904 && longIp <= 453246974) { return false; }
            if (longIp >= 1884160000 && longIp <= 1884164094) { return false; }
            if (longIp >= 2113761280 && longIp <= 2113765374) { return false; }
            if (longIp >= 1744397312 && longIp <= 1744398334) { return false; }
            if (longIp >= 762684416 && longIp <= 762685438) { return false; }
            if (longIp >= 18446744072813500000 && longIp <= 18446744072813500000) { return false; }
            if (longIp >= 18446744072943400000 && longIp <= 18446744072943400000) { return false; }
            if (longIp >= 1997651968 && longIp <= 1997660158) { return false; }
            if (longIp >= 1997512704 && longIp <= 1997516798) { return false; }
            if (longIp >= 1700806656 && longIp <= 1700823038) { return false; }
            if (longIp >= 18446744072813500000 && longIp <= 18446744072813500000) { return false; }
            if (longIp >= 18446744072943400000 && longIp <= 18446744072943400000) { return false; }
            if (longIp >= 1997660160 && longIp <= 1997668350) { return false; }
            if (longIp >= 1997516800 && longIp <= 1997520894) { return false; }
            if (longIp >= 1697972224 && longIp <= 1697988606) { return false; }
            if (longIp >= 234885120 && longIp <= 234889214) { return false; }
            if (longIp >= 1743071232 && longIp <= 1743072254) { return false; }
            if (longIp >= 18446744072444100000 && longIp <= 18446744072444100000) { return false; }
            if (longIp >= 1729189888 && longIp <= 1729190910) { return false; }
            if (longIp >= 762742784 && longIp <= 762743806) { return false; }
            if (longIp >= 18446744072828800000 && longIp <= 18446744072828800000) { return false; }
            if (longIp >= 1743668224 && longIp <= 1743669246) { return false; }
            if (longIp >= 18446744072807500000 && longIp <= 18446744072807500000) { return false; }
            if (longIp >= 1743669248 && longIp <= 1743670270) { return false; }
            if (longIp >= 1899241472 && longIp <= 1899249662) { return false; }
            if (longIp >= 838238208 && longIp <= 838254590) { return false; }
            if (longIp >= 838254592 && longIp <= 838262782) { return false; }
            if (longIp >= 18446744072953800000 && longIp <= 18446744072953800000) { return false; }
            if (longIp >= 985268224 && longIp <= 985399294) { return false; }
            if (longIp >= 1984167936 && longIp <= 1984430078) { return false; }
            if (longIp >= 1897267200 && longIp <= 1897332734) { return false; }
            if (longIp >= 1897332736 && longIp <= 1897365502) { return false; }
            if (longIp >= 18446744072490000000 && longIp <= 18446744072490100000) { return false; }
            if (longIp >= 18446744072490100000 && longIp <= 18446744072490100000) { return false; }
            if (longIp >= 20185088 && longIp <= 20447230) { return false; }
            if (longIp >= 711983104 && longIp <= 712507390) { return false; }
            if (longIp >= 1730363392 && longIp <= 1730364414) { return false; }
            if (longIp >= 737121280 && longIp <= 737122302) { return false; }
            if (longIp >= 18446744072806400000 && longIp <= 18446744072806400000) { return false; }
            if (longIp >= 1729400832 && longIp <= 1729401854) { return false; }
            if (longIp >= 18446744072490700000 && longIp <= 18446744072490700000) { return false; }
            if (longIp >= 1700986880 && longIp <= 1701003262) { return false; }
            if (longIp >= 18446744072833700000 && longIp <= 18446744072833800000) { return false; }
            if (longIp >= 1728693248 && longIp <= 1728694270) { return false; }
            if (longIp >= 1897160704 && longIp <= 1897168894) { return false; }
            if (longIp >= 763029504 && longIp <= 763030526) { return false; }
            if (longIp >= 1025302528 && longIp <= 1025310718) { return false; }
            if (longIp >= 763362304 && longIp <= 763363326) { return false; }
            if (longIp >= 1740958720 && longIp <= 1740959742) { return false; }
            if (longIp >= 1868294144 && longIp <= 1868295166) { return false; }
            if (longIp >= 1729340416 && longIp <= 1729341438) { return false; }
            if (longIp >= 763219968 && longIp <= 763220990) { return false; }
            if (longIp >= 1743335424 && longIp <= 1743336446) { return false; }
            if (longIp >= 763123712 && longIp <= 763124734) { return false; }
            if (longIp >= 1729597440 && longIp <= 1729598462) { return false; }
            if (longIp >= 762743808 && longIp <= 762744830) { return false; }
            if (longIp >= 1742985216 && longIp <= 1742986238) { return false; }
            if (longIp >= 18446744072803900000 && longIp <= 18446744072804000000) { return false; }
            if (longIp >= 18446744072803900000 && longIp <= 18446744072803900000) { return false; }
            if (longIp >= 18446744072808700000 && longIp <= 18446744072808700000) { return false; }
            if (longIp >= 1958821888 && longIp <= 1958825982) { return false; }
            if (longIp >= 2018009088 && longIp <= 2018017278) { return false; }
            if (longIp >= 2018004992 && longIp <= 2018007038) { return false; }
            if (longIp >= 18446744072937900000 && longIp <= 18446744072937900000) { return false; }
            if (longIp >= 18446744072825400000 && longIp <= 18446744072825400000) { return false; }
            if (longIp >= 18446744072812300000 && longIp <= 18446744072812300000) { return false; }
            if (longIp >= 18446744072831500000 && longIp <= 18446744072831500000) { return false; }
            if (longIp >= 2059995136 && longIp <= 2059997182) { return false; }
            if (longIp >= 1986396160 && longIp <= 1986398206) { return false; }
            if (longIp >= 2022326272 && longIp <= 2022330366) { return false; }
            if (longIp >= 836059136 && longIp <= 836075518) { return false; }
            if (longIp >= 18446744072826900000 && longIp <= 18446744072826900000) { return false; }
            if (longIp >= 18446744072814800000 && longIp <= 18446744072814800000) { return false; }
            if (longIp >= 18446744072831500000 && longIp <= 18446744072831500000) { return false; }
            if (longIp >= 1728521216 && longIp <= 1728522238) { return false; }
            if (longIp >= 763256832 && longIp <= 763257854) { return false; }
            if (longIp >= 18446744072809300000 && longIp <= 18446744072809300000) { return false; }
            if (longIp >= 18446744072814000000 && longIp <= 18446744072814000000) { return false; }
            if (longIp >= 18446744072832800000 && longIp <= 18446744072832800000) { return false; }
            if (longIp >= 1950646272 && longIp <= 1950648318) { return false; }
            if (longIp >= 1730115584 && longIp <= 1730116606) { return false; }
            if (longIp >= 1997715456 && longIp <= 1997717502) { return false; }
            if (longIp >= 1986740224 && longIp <= 1986756606) { return false; }
            if (longIp >= 1742958592 && longIp <= 1742959614) { return false; }
            if (longIp >= 737131520 && longIp <= 737132542) { return false; }
            if (longIp >= 18446744072816100000 && longIp <= 18446744072816100000) { return false; }
            if (longIp >= 1742927872 && longIp <= 1742928894) { return false; }
            if (longIp >= 2016589824 && longIp <= 2016591870) { return false; }
            if (longIp >= 18446744072831900000 && longIp <= 18446744072831900000) { return false; }
            if (longIp >= 18446744072478600000 && longIp <= 18446744072478600000) { return false; }
            if (longIp >= 18446744072832700000 && longIp <= 18446744072832700000) { return false; }
            if (longIp >= 1934929920 && longIp <= 1934931966) { return false; }
            if (longIp >= 18446744072951600000 && longIp <= 18446744072951600000) { return false; }
            if (longIp >= 1728172032 && longIp <= 1728173054) { return false; }
            if (longIp >= 762683392 && longIp <= 762684414) { return false; }
            if (longIp >= 1888059392 && longIp <= 1888063486) { return false; }
            if (longIp >= 763217920 && longIp <= 763218942) { return false; }
            if (longIp >= 18446744072833500000 && longIp <= 18446744072833500000) { return false; }
            if (longIp >= 1847805952 && longIp <= 1847807998) { return false; }
            if (longIp >= 1848424448 && longIp <= 1848426494) { return false; }
            if (longIp >= 18446744072812100000 && longIp <= 18446744072812100000) { return false; }
            if (longIp >= 18446744072822600000 && longIp <= 18446744072822600000) { return false; }
            if (longIp >= 18446744072444200000 && longIp <= 18446744072444200000) { return false; }
            if (longIp >= 1866592256 && longIp <= 1866596350) { return false; }
            if (longIp >= 1744443392 && longIp <= 1744444414) { return false; }
            if (longIp >= 18446744072830900000 && longIp <= 18446744072830900000) { return false; }
            if (longIp >= 18446744072803800000 && longIp <= 18446744072803800000) { return false; }
            if (longIp >= 18446744072812300000 && longIp <= 18446744072812300000) { return false; }
            if (longIp >= 18446744072490700000 && longIp <= 18446744072490700000) { return false; }
            if (longIp >= 18446744072803800000 && longIp <= 18446744072803800000) { return false; }
            if (longIp >= 18446744072357500000 && longIp <= 18446744072357500000) { return false; }
            if (longIp >= 18446744072811700000 && longIp <= 18446744072811700000) { return false; }
            if (longIp >= 452987904 && longIp <= 452988926) { return false; }
            if (longIp >= 1728179200 && longIp <= 1728180222) { return false; }
            if (longIp >= 1728522240 && longIp <= 1728523262) { return false; }
            if (longIp >= 762685440 && longIp <= 762686462) { return false; }
            if (longIp >= 1893027840 && longIp <= 1893031934) { return false; }
            if (longIp >= 18446744072807400000 && longIp <= 18446744072807400000) { return false; }
            if (longIp >= 18446744072825500000 && longIp <= 18446744072825500000) { return false; }
            if (longIp >= 18446744072483500000 && longIp <= 18446744072483500000) { return false; }
            if (longIp >= 18446744072807200000 && longIp <= 18446744072807200000) { return false; }
            if (longIp >= 18446744072483500000 && longIp <= 18446744072483500000) { return false; }
            if (longIp >= 1886214144 && longIp <= 1886216190) { return false; }
            if (longIp >= 1743507456 && longIp <= 1743508478) { return false; }
            if (longIp >= 18446744072804100000 && longIp <= 18446744072804100000) { return false; }
            if (longIp >= 18446744072804100000 && longIp <= 18446744072804100000) { return false; }
            if (longIp >= 18446744072804100000 && longIp <= 18446744072804100000) { return false; }
            if (longIp >= 460722176 && longIp <= 460726270) { return false; }
            if (longIp >= 763255808 && longIp <= 763256830) { return false; }
            if (longIp >= 1744147456 && longIp <= 1744148478) { return false; }
            if (longIp >= 18446744072806900000 && longIp <= 18446744072807000000) { return false; }
            if (longIp >= 18446744072807500000 && longIp <= 18446744072807500000) { return false; }
            if (longIp >= 18446744072807500000 && longIp <= 18446744072807500000) { return false; }
            if (longIp >= 2053533696 && longIp <= 2053534718) { return false; }
            if (longIp >= 18446744072808400000 && longIp <= 18446744072808400000) { return false; }
            if (longIp >= 18446744072808400000 && longIp <= 18446744072808400000) { return false; }
            if (longIp >= 832320512 && longIp <= 832321534) { return false; }
            if (longIp >= 1742859264 && longIp <= 1742860286) { return false; }
            if (longIp >= 1731585024 && longIp <= 1731586046) { return false; }
            if (longIp >= 18446744072809700000 && longIp <= 18446744072809700000) { return false; }
            if (longIp >= 1960058880 && longIp <= 1960067070) { return false; }
            if (longIp >= 18446744072803600000 && longIp <= 18446744072803600000) { return false; }
            if (longIp >= 18446744072806000000 && longIp <= 18446744072806000000) { return false; }
            if (longIp >= 1744568320 && longIp <= 1744569342) { return false; }
            if (longIp >= 18446744072806500000 && longIp <= 18446744072806500000) { return false; }
            if (longIp >= 18446744072820900000 && longIp <= 18446744072820900000) { return false; }
            if (longIp >= 18446744072820900000 && longIp <= 18446744072820900000) { return false; }
            if (longIp >= 18446744072832800000 && longIp <= 18446744072832800000) { return false; }
            if (longIp >= 1997701120 && longIp <= 1997705214) { return false; }
            if (longIp >= 1847803904 && longIp <= 1847805950) { return false; }
            if (longIp >= 18446744072490800000 && longIp <= 18446744072490800000) { return false; }
            if (longIp >= 18446744073157600000 && longIp <= 18446744073157700000) { return false; }
            if (longIp >= 1728169984 && longIp <= 1728171006) { return false; }
            if (longIp >= 1728312320 && longIp <= 1728313342) { return false; }
            if (longIp >= 1728313344 && longIp <= 1728314366) { return false; }
            if (longIp >= 762897408 && longIp <= 762898430) { return false; }
            if (longIp >= 1744201728 && longIp <= 1744201982) { return false; }
            if (longIp >= 1899850752 && longIp <= 1899851774) { return false; }
            if (longIp >= 1700793344 && longIp <= 1700794366) { return false; }
            if (longIp >= 1728314368 && longIp <= 1728315390) { return false; }
            if (longIp >= 762945536 && longIp <= 762946558) { return false; }
            if (longIp >= 1728731136 && longIp <= 1728732158) { return false; }
            if (longIp >= 1743000576 && longIp <= 1743001598) { return false; }
            if (longIp >= 1728348160 && longIp <= 1728349182) { return false; }
            if (longIp >= 1728762880 && longIp <= 1728763902) { return false; }
            if (longIp >= 1744231424 && longIp <= 1744232446) { return false; }
            if (longIp >= 762795008 && longIp <= 762796030) { return false; }
            if (longIp >= 1728388608 && longIp <= 1728389118) { return false; }
            if (longIp >= 1728434176 && longIp <= 1728435198) { return false; }
            if (longIp >= 1728433152 && longIp <= 1728434174) { return false; }
            if (longIp >= 1728818176 && longIp <= 1728819198) { return false; }
            if (longIp >= 1729600512 && longIp <= 1729601534) { return false; }
            if (longIp >= 999937024 && longIp <= 999938046) { return false; }
            if (longIp >= 1731842048 && longIp <= 1731843070) { return false; }
            if (longIp >= 762687488 && longIp <= 762688510) { return false; }
            if (longIp >= 1729897472 && longIp <= 1729898494) { return false; }
            if (longIp >= 762662912 && longIp <= 762663934) { return false; }
            if (longIp >= 1729923072 && longIp <= 1729924094) { return false; }
            if (longIp >= 1729688576 && longIp <= 1729689598) { return false; }
            if (longIp >= 1729932288 && longIp <= 1729933310) { return false; }
            if (longIp >= 1883783168 && longIp <= 1883799550) { return false; }
            if (longIp >= 1728562176 && longIp <= 1728562430) { return false; }
            if (longIp >= 1731591168 && longIp <= 1731592190) { return false; }
            if (longIp >= 762910720 && longIp <= 762911742) { return false; }
            if (longIp >= 1728557312 && longIp <= 1728557566) { return false; }
            if (longIp >= 1742043648 && longIp <= 1742044670) { return false; }
            if (longIp >= 1728556032 && longIp <= 1728556286) { return false; }
            if (longIp >= 1728580864 && longIp <= 1728581118) { return false; }
            if (longIp >= 1728663552 && longIp <= 1728664574) { return false; }
            if (longIp >= 1728240640 && longIp <= 1728241662) { return false; }
            if (longIp >= 1728664576 && longIp <= 1728665598) { return false; }
            if (longIp >= 1728643072 && longIp <= 1728644094) { return false; }
            if (longIp >= 1728662528 && longIp <= 1728663550) { return false; }
            if (longIp >= 18446744071876900000 && longIp <= 18446744071876900000) { return false; }
            if (longIp >= 1728644096 && longIp <= 1728645118) { return false; }
            if (longIp >= 1728694272 && longIp <= 1728695294) { return false; }
            if (longIp >= 1728697344 && longIp <= 1728698366) { return false; }
            if (longIp >= 737140736 && longIp <= 737141758) { return false; }
            if (longIp >= 1743397888 && longIp <= 1743398910) { return false; }
            if (longIp >= 1728695296 && longIp <= 1728696318) { return false; }
            if (longIp >= 1728924672 && longIp <= 1728925694) { return false; }
            if (longIp >= 1728866304 && longIp <= 1728867326) { return false; }
            if (longIp >= 1938978816 && longIp <= 1938980862) { return false; }
            if (longIp >= 2090729472 && longIp <= 2090731518) { return false; }
            if (longIp >= 1729467392 && longIp <= 1729468414) { return false; }
            if (longIp >= 1940234240 && longIp <= 1940236286) { return false; }
            if (longIp >= 2090731520 && longIp <= 2090733566) { return false; }
            if (longIp >= 1729401856 && longIp <= 1729402878) { return false; }
            if (longIp >= 762745856 && longIp <= 762746878) { return false; }
            if (longIp >= 1729048576 && longIp <= 1729049598) { return false; }
            if (longIp >= 1743508480 && longIp <= 1743509502) { return false; }
            if (longIp >= 1729460224 && longIp <= 1729461246) { return false; }
            if (longIp >= 1730028544 && longIp <= 1730029566) { return false; }
            if (longIp >= 1730116608 && longIp <= 1730117630) { return false; }
            if (longIp >= 1729277952 && longIp <= 1729278974) { return false; }
            if (longIp >= 1729227776 && longIp <= 1729228798) { return false; }
            if (longIp >= 1729323008 && longIp <= 1729324030) { return false; }
            if (longIp >= 1729354752 && longIp <= 1729355774) { return false; }
            if (longIp >= 1729101824 && longIp <= 1729102846) { return false; }
            if (longIp >= 1744347136 && longIp <= 1744348158) { return false; }
            if (longIp >= 1744348160 && longIp <= 1744349182) { return false; }
            if (longIp >= 1744376832 && longIp <= 1744377854) { return false; }
            if (longIp >= 763026432 && longIp <= 763027454) { return false; }
            if (longIp >= 1729838080 && longIp <= 1729839102) { return false; }
            if (longIp >= 1729839104 && longIp <= 1729840126) { return false; }
            if (longIp >= 1729821696 && longIp <= 1729822718) { return false; }
            if (longIp >= 1729883136 && longIp <= 1729884158) { return false; }
            if (longIp >= 762793984 && longIp <= 762795006) { return false; }
            if (longIp >= 18446744072809600000 && longIp <= 18446744072809600000) { return false; }
            if (longIp >= 1731845120 && longIp <= 1731846142) { return false; }
            if (longIp >= 18446744071833600000 && longIp <= 18446744071833600000) { return false; }
            if (longIp >= 1743910912 && longIp <= 1743911934) { return false; }
            if (longIp >= 1743926272 && longIp <= 1743927294) { return false; }
            if (longIp >= 1744033792 && longIp <= 1744034814) { return false; }
            if (longIp >= 1729233920 && longIp <= 1729234942) { return false; }
            if (longIp >= 18446744071716900000 && longIp <= 18446744071716900000) { return false; }
            if (longIp >= 1744005120 && longIp <= 1744006142) { return false; }
            if (longIp >= 1744656384 && longIp <= 1744657406) { return false; }
            if (longIp >= 1744709632 && longIp <= 1744710654) { return false; }
            if (longIp >= 1744703488 && longIp <= 1744704510) { return false; }
            if (longIp >= 1744702464 && longIp <= 1744703486) { return false; }
            if (longIp >= 762686464 && longIp <= 762687486) { return false; }
            if (longIp >= 1744754688 && longIp <= 1744755710) { return false; }
            if (longIp >= 762801152 && longIp <= 762802174) { return false; }
            if (longIp >= 1744786432 && longIp <= 1744787454) { return false; }
            if (longIp >= 762792960 && longIp <= 762793982) { return false; }
            if (longIp >= 1744825344 && longIp <= 1744826366) { return false; }
            if (longIp >= 1024387072 && longIp <= 1024388094) { return false; }
            if (longIp >= 1742776320 && longIp <= 1742777342) { return false; }
            if (longIp >= 1742892032 && longIp <= 1742893054) { return false; }
            if (longIp >= 1744632832 && longIp <= 1744633854) { return false; }
            if (longIp >= 1744172032 && longIp <= 1744173054) { return false; }
            if (longIp >= 18446744072831300000 && longIp <= 18446744072831300000) { return false; }
            if (longIp >= 18446744072448500000 && longIp <= 18446744072448500000) { return false; }
            if (longIp >= 1744173056 && longIp <= 1744174078) { return false; }
            if (longIp >= 978583552 && longIp <= 978584574) { return false; }
            if (longIp >= 1744174080 && longIp <= 1744175102) { return false; }
            if (longIp >= 1743110144 && longIp <= 1743111166) { return false; }
            if (longIp >= 1743229952 && longIp <= 1743230974) { return false; }
            if (longIp >= 1743272960 && longIp <= 1743273982) { return false; }
            if (longIp >= 1743271936 && longIp <= 1743272958) { return false; }
            if (longIp >= 1743270912 && longIp <= 1743271934) { return false; }
            if (longIp >= 1743288320 && longIp <= 1743289342) { return false; }
            if (longIp >= 1741096960 && longIp <= 1741097982) { return false; }
            if (longIp >= 999938048 && longIp <= 999939070) { return false; }
            if (longIp >= 1743600640 && longIp <= 1743601662) { return false; }
            if (longIp >= 1743609856 && longIp <= 1743610878) { return false; }
            if (longIp >= 1743601664 && longIp <= 1743602686) { return false; }
            if (longIp >= 763218944 && longIp <= 763219966) { return false; }
            if (longIp >= 1743623168 && longIp <= 1743624190) { return false; }
            if (longIp >= 1743622144 && longIp <= 1743623166) { return false; }
            if (longIp >= 762744832 && longIp <= 762745854) { return false; }
            if (longIp >= 1743704064 && longIp <= 1743705086) { return false; }
            if (longIp >= 1208020992 && longIp <= 1208022014) { return false; }
            if (longIp >= 1743671296 && longIp <= 1743672318) { return false; }
            if (longIp >= 796236800 && longIp <= 796237822) { return false; }
            if (longIp >= 1743670272 && longIp <= 1743671294) { return false; }
            if (longIp >= 1743705088 && longIp <= 1743706110) { return false; }
            if (longIp >= 762661888 && longIp <= 762662910) { return false; }
            if (longIp >= 1743724544 && longIp <= 1743725566) { return false; }
            if (longIp >= 1743747072 && longIp <= 1743748094) { return false; }
            if (longIp >= 762834944 && longIp <= 762835966) { return false; }
            if (longIp >= 1743746048 && longIp <= 1743747070) { return false; }
            if (longIp >= 1730486272 && longIp <= 1730487294) { return false; }
            if (longIp >= 1730485248 && longIp <= 1730486270) { return false; }
            if (longIp >= 1730578432 && longIp <= 1730579454) { return false; }
            if (longIp >= 1730632704 && longIp <= 1730633726) { return false; }
            if (longIp >= 1730633728 && longIp <= 1730634750) { return false; }
            if (longIp >= 1728731136 && longIp <= 1728732158) { return false; }
            if (longIp >= 18446744072812900000 && longIp <= 18446744072812900000) { return false; }
            if (longIp >= 1730820096 && longIp <= 1730821118) { return false; }
            if (longIp >= 1731060736 && longIp <= 1731061758) { return false; }
            if (longIp >= 1731062784 && longIp <= 1731063806) { return false; }
            if (longIp >= 1731061760 && longIp <= 1731062782) { return false; }
            if (longIp >= 1731182592 && longIp <= 1731183614) { return false; }
            if (longIp >= 763227136 && longIp <= 763228158) { return false; }
            if (longIp >= 1731218432 && longIp <= 1731219454) { return false; }
            if (longIp >= 1731220480 && longIp <= 1731221502) { return false; }
            if (longIp >= 1731219456 && longIp <= 1731220478) { return false; }
            if (longIp >= 763122688 && longIp <= 763123710) { return false; }
            if (longIp >= 1731247104 && longIp <= 1731248126) { return false; }
            if (longIp >= 1731248128 && longIp <= 1731249150) { return false; }
            if (longIp >= 762827776 && longIp <= 762828798) { return false; }
            if (longIp >= 1731484672 && longIp <= 1731485694) { return false; }
            if (longIp >= 1731549184 && longIp <= 1731550206) { return false; }
            if (longIp >= 737139712 && longIp <= 737140734) { return false; }
            if (longIp >= 1731569664 && longIp <= 1731570686) { return false; }
            if (longIp >= 1741935616 && longIp <= 1741936638) { return false; }
            if (longIp >= 1731655680 && longIp <= 1731656702) { return false; }
            if (longIp >= 1731766272 && longIp <= 1731767294) { return false; }
            if (longIp >= 1731763200 && longIp <= 1731764222) { return false; }
            if (longIp >= 763124736 && longIp <= 763125758) { return false; }
            if (longIp >= 1731764224 && longIp <= 1731765246) { return false; }
            if (longIp >= 1731765248 && longIp <= 1731766270) { return false; }
            if (longIp >= 1731815424 && longIp <= 1731816446) { return false; }
            if (longIp >= 1731817472 && longIp <= 1731818494) { return false; }
            if (longIp >= 1731989504 && longIp <= 1731990526) { return false; }
            if (longIp >= 762828800 && longIp <= 762829822) { return false; }
            if (longIp >= 1732062208 && longIp <= 1732063230) { return false; }
            if (longIp >= 762896384 && longIp <= 762897406) { return false; }
            if (longIp >= 1732063232 && longIp <= 1732064254) { return false; }
            if (longIp >= 1732118528 && longIp <= 1732119550) { return false; }
            if (longIp >= 762943488 && longIp <= 762944510) { return false; }
            if (longIp >= 1732236288 && longIp <= 1732237310) { return false; }
            if (longIp >= 763060224 && longIp <= 763061246) { return false; }
            if (longIp >= 1732212736 && longIp <= 1732213758) { return false; }
            if (longIp >= 763030528 && longIp <= 763031550) { return false; }
            if (longIp >= 1732211712 && longIp <= 1732212734) { return false; }
            if (longIp >= 763031552 && longIp <= 763032574) { return false; }
            if (longIp >= 1732208640 && longIp <= 1732209662) { return false; }
            if (longIp >= 763033600 && longIp <= 763034622) { return false; }
            if (longIp >= 1732209664 && longIp <= 1732210686) { return false; }
            if (longIp >= 763034624 && longIp <= 763035646) { return false; }
            if (longIp >= 1732210688 && longIp <= 1732211710) { return false; }
            if (longIp >= 763032576 && longIp <= 763033598) { return false; }
            if (longIp >= 1740696576 && longIp <= 1740697598) { return false; }
            if (longIp >= 1740894208 && longIp <= 1740895230) { return false; }
            if (longIp >= 1740893184 && longIp <= 1740894206) { return false; }
            if (longIp >= 1740902400 && longIp <= 1740903422) { return false; }
            if (longIp >= 1740960768 && longIp <= 1740961790) { return false; }
            if (longIp >= 1740961792 && longIp <= 1740962814) { return false; }
            if (longIp >= 1741100032 && longIp <= 1741101054) { return false; }
            if (longIp >= 999939072 && longIp <= 999940094) { return false; }
            if (longIp >= 1741101056 && longIp <= 1741102078) { return false; }
            if (longIp >= 999940096 && longIp <= 999941118) { return false; }
            if (longIp >= 1741102080 && longIp <= 1741103102) { return false; }
            if (longIp >= 999941120 && longIp <= 999942142) { return false; }
            if (longIp >= 1741103104 && longIp <= 1741104126) { return false; }
            if (longIp >= 999942144 && longIp <= 999943166) { return false; }
            if (longIp >= 1741105152 && longIp <= 1741106174) { return false; }
            if (longIp >= 999943168 && longIp <= 999944190) { return false; }
            if (longIp >= 1741104128 && longIp <= 1741105150) { return false; }
            if (longIp >= 999944192 && longIp <= 999945214) { return false; }
            if (longIp >= 1741106176 && longIp <= 1741107198) { return false; }
            if (longIp >= 999945216 && longIp <= 999946238) { return false; }
            if (longIp >= 1741108224 && longIp <= 1741109246) { return false; }
            if (longIp >= 999946240 && longIp <= 999947262) { return false; }
            if (longIp >= 1741107200 && longIp <= 1741108222) { return false; }
            if (longIp >= 999947264 && longIp <= 999948286) { return false; }
            if (longIp >= 1741109248 && longIp <= 1741110270) { return false; }
            if (longIp >= 18446744071716900000 && longIp <= 18446744071716900000) { return false; }
            if (longIp >= 1741110272 && longIp <= 1741111294) { return false; }
            if (longIp >= 18446744071716900000 && longIp <= 18446744071716900000) { return false; }
            if (longIp >= 1741112320 && longIp <= 1741113342) { return false; }
            if (longIp >= 18446744071716900000 && longIp <= 18446744071716900000) { return false; }
            if (longIp >= 1741113344 && longIp <= 1741114366) { return false; }
            if (longIp >= 18446744071716900000 && longIp <= 18446744071716900000) { return false; }
            if (longIp >= 1741114368 && longIp <= 1741115390) { return false; }
            if (longIp >= 18446744071716900000 && longIp <= 18446744071716900000) { return false; }
            if (longIp >= 1741111296 && longIp <= 1741112318) { return false; }
            if (longIp >= 18446744071716900000 && longIp <= 18446744071716900000) { return false; }
            if (longIp >= 1741099008 && longIp <= 1741100030) { return false; }
            if (longIp >= 1741095936 && longIp <= 1741096958) { return false; }
            if (longIp >= 1741097984 && longIp <= 1741099006) { return false; }
            if (longIp >= 1741165568 && longIp <= 1741166590) { return false; }
            if (longIp >= 1741166592 && longIp <= 1741167614) { return false; }
            if (longIp >= 18446744072831300000 && longIp <= 18446744072831300000) { return false; }
            if (longIp >= 1741191168 && longIp <= 1741192190) { return false; }
            if (longIp >= 2033363968 && longIp <= 2033364990) { return false; }
            if (longIp >= 1728719872 && longIp <= 1728720894) { return false; }
            if (longIp >= 18446744072830800000 && longIp <= 18446744072830800000) { return false; }
            if (longIp >= 1728683008 && longIp <= 1728684030) { return false; }
            if (longIp >= 1741513728 && longIp <= 1741514750) { return false; }
            if (longIp >= 1741514752 && longIp <= 1741515774) { return false; }
            if (longIp >= 1741512704 && longIp <= 1741513726) { return false; }
            if (longIp >= 1024388096 && longIp <= 1024389118) { return false; }
            if (longIp >= 1741608960 && longIp <= 1741609982) { return false; }
            if (longIp >= 1741607936 && longIp <= 1741608958) { return false; }
            if (longIp >= 18446744071716900000 && longIp <= 18446744071716900000) { return false; }
            if (longIp >= 1741628416 && longIp <= 1741629438) { return false; }
            if (longIp >= 1741627392 && longIp <= 1741628414) { return false; }
            if (longIp >= 1742080000 && longIp <= 1742081022) { return false; }
            if (longIp >= 18446744071833600000 && longIp <= 18446744071833600000) { return false; }
            return true;
        }

        /// <summary>
        /// Get local ip, using for winform and webform
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp()
        {
            string ip = null;

            // Resolves a host name or IP address to an IPHostEntry instance.
            // IPHostEntry - Provides a container class for Internet host address information. 
            System.Net.IPHostEntry ipHostEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

            // IPAddress class contains the address of a computer on an IP network. 
            foreach (System.Net.IPAddress ipAddress in ipHostEntry.AddressList)
            {
                // InterNetwork indicates that an IP version 4 address is expected 
                // when a Socket connects to an endpoint
                if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    ip = ipAddress.ToString();
                }
            }
            return ip;
        }

        /// <summary>
        /// Gửi email
        /// </summary>
        /// <param name="fromEmail">Địa chỉ gửi đi</param>
        /// <param name="fromName">Tên người gửi</param>
        /// <param name="fromPassword">Mật khẩu đăng nhập mail của người gửi</param>
        /// <param name="toEmail">Địa chỉ nhận mail</param>
        /// <param name="toName">Tên người nhận</param>
        /// <param name="title">Tiêu đề</param>
        /// <param name="body">Nội dung</param>
        /// <param name="host">Host của email. Gmail là "smtp.gmail.com"</param>
        /// <param name="port">Cổng. Nếu là Gmail thì là 587</param>
        /// <param name="isEnableSsl">Có mở chế độ SSL không</param>
        /// <param name="ccAddresses"></param>
        /// <param name="bccAddresses"></param>
        /// <returns></returns>
        public static bool SendMail(string fromEmail, string fromName, string fromPassword,
                                    string toEmail, string toName, string title, string body,
                                    string host, int port, bool isEnableSsl = false,
                                    List<MailAddress> ccAddresses = null, List<MailAddress> bccAddresses = null)
        {
            var fromAddress = new MailAddress(fromEmail, fromName);
            var toAddress = new MailAddress(toEmail, toName);

            return SendMail(fromAddress, toAddress, fromPassword, title, body, host, port, isEnableSsl,
                ccAddresses, bccAddresses);
        }

        /// <summary>
        /// Gửi email
        /// </summary>
        /// <param name="fromMail">Địa chỉ gửi đi</param>
        /// <param name="fromPassword">Mật khẩu đăng nhập mail của người gửi</param>
        /// <param name="toEmail">Địa chỉ nhận mail</param>
        /// <param name="replyEmail">Địa chỉ reply</param>
        /// <param name="title">Tiêu đề</param>
        /// <param name="body">Nội dung</param>
        /// <param name="host">Host của email. Gmail là "smtp.gmail.com"</param>
        /// <param name="port">Cổng. Nếu là Gmail thì là 587</param>
        /// <param name="isEnableSsl">Có mở chế độ SSL không</param>
        /// <param name="ccAddresses"></param>
        /// <param name="bccAddresses"></param>
        /// <returns></returns>
        public static bool SendMail(MailAddress fromMail, MailAddress toEmail,
                                    string fromPassword, string title, string body, string host, int port,
                                    bool isEnableSsl = false, List<MailAddress> ccAddresses = null,
                                    List<MailAddress> bccAddresses = null, MailAddress replyEmail = null)
        {
            using (var message = new MailMessage(fromMail, toEmail)
            {
                Subject = title,
                Body = body,
                IsBodyHtml = true,
            })
            {
                if (replyEmail != null)
                {
                    message.ReplyTo = replyEmail;
                }

                if (ccAddresses != null)
                {
                    foreach (var address in ccAddresses)
                    {
                        message.Bcc.Add(address);
                    }
                }

                if (bccAddresses != null)
                {
                    foreach (var address in bccAddresses)
                    {
                        message.CC.Add(address);
                    }
                }

                var smtp = new SmtpClient(host, port)
                {
                    EnableSsl = isEnableSsl,
                    Credentials = new NetworkCredential(fromMail.Address, fromPassword),
                };
                smtp.Send(message);
            }
            return true;
        }

        /// <summary>
        /// Hàm gửi request dạng Get
        /// </summary>
        /// <param name="requestUrl">Địa chỉ cần gọi</param>
        /// <param name="cookieCon">Cookie</param>
        /// <param name="htmlResult">Kết quả html trả về</param>
        /// <param name="referer">Địa chỉ tiếp theo, nếu có</param>
        /// <param name="nextLocation">Địa chỉ tiếp theo, nếu có</param>
        /// <param name="allowRedirect">Cho phép tự redirect không</param>
        /// <param name="timeOut">Thời gian time out. Tính bằng giây. Mặc định là 30s</param>
        /// <returns></returns>
        public static bool SendGetRequest(string requestUrl, CookieContainer cookieCon, string referer, out string htmlResult
            , out string nextLocation, bool allowRedirect, int timeOut = 30)
        {
            bool loadSuccess = true;
            nextLocation = string.Empty;
            string resultOutput = string.Empty;
            var webRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

            webRequest.AllowAutoRedirect = false;
            webRequest.AutomaticDecompression = DecompressionMethods.GZip;
            webRequest.CookieContainer = cookieCon;

            // Time out is miliécond
            webRequest.Timeout = timeOut * 1000;

            webRequest.UserAgent = "User-Agent: Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.63 Safari/535.7";
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            webRequest.Headers[HttpRequestHeader.AcceptLanguage] = "en-us,en;q=0.5";
            webRequest.Headers[HttpRequestHeader.AcceptEncoding] = "gzip,deflate";
            webRequest.Headers[HttpRequestHeader.AcceptCharset] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            webRequest.KeepAlive = false; //Fix "The server committed a protocol violation. Section=ResponseStatusLine"
            if (!string.IsNullOrEmpty(referer.Trim()))
                webRequest.Referer = referer;
            webRequest.ContentType = "application/x-www-form-urlencoded";

            webRequest.Method = "GET";

            try
            {
                using (var response = (HttpWebResponse)webRequest.GetResponse())
                {
                    cookieCon.Add(webRequest.RequestUri, response.Cookies);
                    nextLocation = response.GetResponseHeader("Location");
                    using (var buffer = new BufferedStream(response.GetResponseStream()))
                    {
                        using (var readStream = new StreamReader(buffer, Encoding.UTF8))
                        {
                            resultOutput = readStream.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    using (Stream stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        resultOutput = reader.ReadToEnd();
                    }
                }
                loadSuccess = false;
            }
            finally
            {
                htmlResult = resultOutput;
            }
            return loadSuccess;
        }

        /// <summary>
        /// Hàm gửi request dạng Post có header
        /// </summary>
        /// <param name="requestUrl">Địa chỉ cần gọi</param>
        /// <param name="cookieCon">Cookie</param>
        /// <param name="postData">Dữ liệu post</param>
        /// <param name="referer">Địa chỉ liên quan được gọi trước đó</param>
        /// <param name="htmlResult">Kết quả html trả về</param>
        /// <param name="nextLocation">Địa chỉ tiếp theo, nếu có</param>
        /// <param name="allowRedirect">Cho phép tự redirect không</param>
        /// <param name="headers">Thông tin Header</param>
        /// <param name="timeOut">Thời gian time out. Tính bằng giây. Mặc định là 30s</param>
        /// <returns></returns>
        public static bool SendPostRequest(string requestUrl, CookieContainer cookieCon, string postData, string referer
            , out string htmlResult, out string nextLocation, bool allowRedirect, IEnumerable<string> headers, int timeOut = 30)
        {
            bool loadSuccess = true;
            nextLocation = string.Empty;
            string resultOutput = string.Empty;
            byte[] dataByte = new ASCIIEncoding().GetBytes(postData);
            var myRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            myRequest.Method = "POST";
            myRequest.KeepAlive = true;
            myRequest.AllowAutoRedirect = allowRedirect;
            myRequest.Headers.Add("Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.7");
            myRequest.Headers.Add("Keep-Alive:15");

            // Time out is miliécond
            myRequest.Timeout = timeOut * 1000;

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    myRequest.Headers.Add(header);
                }
            }

            myRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            myRequest.ContentType = "application/json";
            if (!string.IsNullOrEmpty(referer.Trim()))
            {
                myRequest.Referer = referer;
            }
            myRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:2.0) Gecko/20100101 Firefox/4.0";
            myRequest.ContentLength = postData.Length;
            myRequest.Proxy = null;
            myRequest.CookieContainer = cookieCon;
            try
            {
                Stream postStream = myRequest.GetRequestStream();
                postStream.Write(dataByte, 0, dataByte.Length);
                postStream.Flush();
                postStream.Close();
                using (var response = (HttpWebResponse)myRequest.GetResponse())
                {
                    cookieCon.Add(myRequest.RequestUri, response.Cookies);
                    nextLocation = response.GetResponseHeader("Location");
                    htmlResult = string.Empty;
                    using (var buffer = new BufferedStream(response.GetResponseStream()))
                    {
                        using (var readStream = new StreamReader(buffer, Encoding.UTF8))
                        {
                            resultOutput = readStream.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loadSuccess = false;
                resultOutput = ex.Message;
            }
            finally
            {
                htmlResult = resultOutput;
            }
            return loadSuccess;
        }

        /// <summary>
        /// Hàm gửi request dạng Post
        /// </summary>
        /// <param name="requestUrl">Địa chỉ cần gọi</param>
        /// <param name="cookieCon">Cookie</param>
        /// <param name="postData">Dữ liệu post</param>
        /// <param name="referer">Địa chỉ liên quan được gọi trước đó</param>
        /// <param name="htmlResult">Kết quả html trả về</param>
        /// <param name="nextLocation">Địa chỉ tiếp theo, nếu có</param>
        /// <param name="allowRedirect">Cho phép tự redirect không</param>
        /// <param name="timeOut">Thời gian time out. Tính bằng giây. Mặc định là 30s</param>
        /// <returns></returns>
        public static bool SendPostRequest(string requestUrl, CookieContainer cookieCon, string postData, string referer
            , out string htmlResult, out string nextLocation, bool allowRedirect, int timeOut = 30)
        {
            bool loadSuccess = true;
            nextLocation = string.Empty;
            string resultOutput = string.Empty;
            byte[] dataByte = new ASCIIEncoding().GetBytes(postData);
            var myRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            myRequest.Method = "POST";
            myRequest.KeepAlive = true;
            myRequest.AllowAutoRedirect = allowRedirect;
            myRequest.Headers.Add("Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.7");
            myRequest.Headers.Add("Keep-Alive:15");
            myRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            myRequest.ContentType = "application/x-www-form-urlencoded";

            // Time out is miliécond
            myRequest.Timeout = timeOut * 1000;

            if (!string.IsNullOrEmpty(referer) && !string.IsNullOrEmpty(referer.Trim()))
            {
                myRequest.Referer = referer;
            }

            myRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:2.0) Gecko/20100101 Firefox/4.0";
            myRequest.ContentLength = postData.Length;
            myRequest.Proxy = null;
            myRequest.CookieContainer = cookieCon;
            try
            {
                Stream postStream = myRequest.GetRequestStream();
                postStream.Write(dataByte, 0, dataByte.Length);
                postStream.Flush();
                postStream.Close();
                using (var response = (HttpWebResponse)myRequest.GetResponse())
                {
                    cookieCon.Add(myRequest.RequestUri, response.Cookies);
                    nextLocation = response.GetResponseHeader("Location");
                    htmlResult = string.Empty;
                    using (var buffer = new BufferedStream(response.GetResponseStream()))
                    {
                        using (var readStream = new StreamReader(buffer, Encoding.UTF8))
                        {
                            resultOutput = readStream.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                htmlResult = resultOutput;
            }
            return loadSuccess;
        }

        /// <summary>
        /// <para>Author: TrungTT</para>
        /// <para>Date: 2015-04-16</para>
        /// <para>Description: Goi request data</para>
        /// </summary>
        /// <returns></returns>
        public static string CallRequest(string urlCall, HttpRequestEnum method, string postData = "")
        {
            var wRequest = WebRequest.Create(urlCall);
            wRequest.Method = Extensions.EnumExtension.Text(method);

            if (method == HttpRequestEnum.Post && postData != "")
            {
                var byteArray = Encoding.UTF8.GetBytes(postData);
                wRequest.ContentType = "application/x-www-form-urlencoded";
                wRequest.ContentLength = byteArray.Length;

                var streamRequest = wRequest.GetRequestStream();
                streamRequest.Write(byteArray, 0, byteArray.Length);
                streamRequest.Close();
            }

            var responseFromServer = string.Empty;

            try
            {
                var wResponse = wRequest.GetResponse();
                var streamResponse = wResponse.GetResponseStream();

                if (streamResponse != null)
                {
                    var reader = new StreamReader(streamResponse);
                    responseFromServer = reader.ReadToEnd();

                    reader.Close();
                    streamResponse.Close();
                }

                wResponse.Close();
            }
            catch { responseFromServer = string.Empty; }

            return responseFromServer;
        }
        /// <summary>
        /// Convert IPv4 address sang Long
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static long Ip2Long(string ipAddress)
        {
            IPAddress ip;

            if (IPAddress.TryParse(ipAddress, out ip))
            {
                return (((long)ip.GetAddressBytes()[0] << 24) | (ip.GetAddressBytes()[1] << 16) |
                        (ip.GetAddressBytes()[2] << 8) | ip.GetAddressBytes()[3]);
            }
            return -1;
        }

        /// <summary>
        /// Convert Long sang IPv4
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static string Long2Ip(long ipAddress)
        {
            IPAddress ip;
            return IPAddress.TryParse(ipAddress.ToString(CultureInfo.InvariantCulture), out ip) ? ip.ToString() : "";
        }
        /// <summary>
        /// Kiem tra xem co phai la IP nuoc ngoai hay ko
        /// <para>true : IP nuoc ngoai</para>
        /// <para>false : IP Vietnam</para>
        /// </summary>
        /// <param name="apiUrl">Link api kiem tra Ip location : http://ip-api.com/json/[ip]</param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsForgeinIp(string apiUrl, string ip)
        {
            if (ip == "localhost" || ip == "::1" || ip == "127.0.0.1") return true;

            string html, next = "";
            if (!SendGetRequest("http://ip-api.com/json/" + ip, new CookieContainer(), "", out html, out next, false))
                return false;
            if (string.IsNullOrEmpty(html)) return false;
            return html.IndexOf("\"countryCode\":\"VN\"", StringComparison.Ordinal) < 0;
        }

        /// <summary>
        /// xử lý send 1 GET request
        /// </summary>
        /// <param name="requestURL"></param>
        /// <returns></returns>
        public static string SendGetRequest(string requestURL, string header = null)
        {
            string resultOutput = string.Empty;
            string nextLocation = "";
            string htmlResult = "";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(requestURL);
            myRequest.Method = "GET";
            myRequest.KeepAlive = true;

            myRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            myRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:2.0) Gecko/20100101 Firefox/4.0";
            myRequest.Headers.Add("Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.7");
            myRequest.Headers.Add("Keep-Alive:15");

            if (!string.IsNullOrEmpty(header))
            {
                myRequest.Headers.Add(header);
            }

            myRequest.Proxy = null;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse())
                {
                    nextLocation = response.GetResponseHeader("Location");

                    htmlResult = string.Empty;

                    using (BufferedStream buffer = new BufferedStream(response.GetResponseStream()))
                    {
                        using (StreamReader readStream = new StreamReader(buffer, Encoding.UTF8))
                        {
                            resultOutput = readStream.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                htmlResult = resultOutput;
            }
            return htmlResult;
        }

        /// <summary>
        /// Hàm gửi request dạng Post
        /// </summary>
        /// <param name="requestUrl">Địa chỉ cần gọi</param>
        /// <param name="cookieCon">Cookie</param>
        /// <param name="postData">Dữ liệu post</param>
        /// <param name="referer">Địa chỉ liên quan được gọi trước đó</param>
        /// <param name="htmlResult">Kết quả html trả về</param>
        /// <param name="nextLocation">Địa chỉ tiếp theo, nếu có</param>
        /// <param name="allowRedirect">Cho phép tự redirect không</param>
        /// <param name="timeOut">Thời gian time out. Tính bằng giây. Mặc định là 30s</param>
        /// <returns></returns>
        public static bool SendPostNotification(string requestUrl, CookieContainer cookieCon, string postData, string keyServer, string referer
            , out string htmlResult, out string nextLocation, bool allowRedirect, int timeOut = 30)
        {
            bool loadSuccess = true;
            nextLocation = string.Empty;
            string resultOutput = string.Empty;
            byte[] dataByte = Encoding.UTF8.GetBytes(postData);
            var myRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            myRequest.Method = "POST";
            myRequest.KeepAlive = true;
            myRequest.AllowAutoRedirect = allowRedirect;
            myRequest.Headers.Add("Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.7");
            myRequest.Headers.Add("Keep-Alive:15");
            myRequest.Headers.Add("ContentType:application/json;charset=utf-8;text/plain");
            myRequest.Headers.Add("Authorization", keyServer);
            myRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            myRequest.ContentType = "application/json; charset=utf-8";

            // Time out is miliécond
            myRequest.Timeout = timeOut * 1000;

            if (!string.IsNullOrEmpty(referer.Trim()))
            {
                myRequest.Referer = referer;
            }
            myRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:2.0) Gecko/20100101 Firefox/4.0";
            myRequest.ContentLength = dataByte.Length;
            myRequest.Proxy = null;
            myRequest.CookieContainer = cookieCon;
            try
            {
                Stream postStream = myRequest.GetRequestStream();
                postStream.Write(dataByte, 0, dataByte.Length);
                postStream.Flush();
                postStream.Close();
                using (var response = (HttpWebResponse)myRequest.GetResponse())
                {
                    cookieCon.Add(myRequest.RequestUri, response.Cookies);
                    nextLocation = response.GetResponseHeader("Location");
                    htmlResult = string.Empty;
                    using (var buffer = new BufferedStream(response.GetResponseStream()))
                    {
                        using (var readStream = new StreamReader(buffer, Encoding.UTF8))
                        {
                            resultOutput = readStream.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loadSuccess = false;
            }
            finally
            {
                htmlResult = resultOutput;
            }
            return loadSuccess;
        }

        /// <summary>
        /// xử lý send 1 GET request
        /// </summary>
        /// <param name="requestURL"></param>
        /// <returns></returns>
        public static string SendGetRequestSimple(string requestURL)
        {
            string resultOutput = string.Empty;
            string htmlResult = string.Empty;
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(requestURL);
            myRequest.Method = "GET";
            myRequest.KeepAlive = true;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse())
                {
                    htmlResult = string.Empty;

                    using (BufferedStream buffer = new BufferedStream(response.GetResponseStream()))
                    {
                        using (StreamReader readStream = new StreamReader(buffer, Encoding.UTF8))
                        {
                            resultOutput = readStream.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    using (Stream stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        resultOutput = reader.ReadToEnd();
                    }
                }
            }
            finally
            {
                htmlResult = resultOutput;
            }

            return htmlResult;
        }

        public static string SendPOSTRequestJsonSimple(string url, string formBody, string contentType = "application/json")
        {
            string resultOutput = string.Empty;
            string htmlResult = string.Empty;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                var encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(formBody);
                request.ContentType = contentType;
                request.ContentLength = data.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(data, 0, data.Length);
                writer.Close();

                var response = (HttpWebResponse)request.GetResponse();
                htmlResult = string.Empty;

                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            resultOutput = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                resultOutput = e.InnerException != null ? e.InnerException.ToString() : e.Message;
                using (var response = e.Response)
                {
                    if (response != null)
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            if (stream != null)
                            {
                                using (var reader = new StreamReader(stream))
                                {
                                    resultOutput = reader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                resultOutput = exp.ToString();
            }
            finally
            {
                htmlResult = resultOutput;
            }

            return htmlResult;
        }


        /// <summary>
        /// Url encode param
        /// </summary>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static string GetUrlEncode(string paramValue)
        {
            return HttpUtility.UrlEncode(paramValue);
        }

        /// <summary>
        /// Url decode param
        /// </summary>
        /// <returns></returns>
        public static string GetUrlDecode(string paramValue)
        {
            return HttpUtility.UrlEncode(paramValue);
        }
    }
}