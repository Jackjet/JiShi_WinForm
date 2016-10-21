using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JiShi_WinForm
{
    public partial class RedisMonitor : Form
    {

        bool working = false;
        string UserName = ConfigurationManager.AppSettings["FTPUserName"];
        string Password = ConfigurationManager.AppSettings["FTPPassword"];
        System.Uri Uri = new Uri("ftp://" + ConfigurationManager.AppSettings["FTPServer"] + ":" + ConfigurationManager.AppSettings["FTPPortNO"]);
        IDatabase db = SeRedis.redis.GetDatabase();
        public RedisMonitor()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
            this.button1.Text = "运行中";
            this.button1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!working)
            {
                working = true;
                InsertDecl();
                working = false;
            }
        }
        private void InsertDeclList()
        {
            string json_decllist = db.ListLeftPop("Redis_DeclareList");
            if (!string.IsNullOrEmpty(json_decllist))
            {
                JObject jo_decllist = (JObject)JsonConvert.DeserializeObject(json_decllist);
                string sql = "insert into list_decllist() values()";
            }
        }
        private void InsertDecl()
        {
            string json_decl = db.ListLeftPop("Redis_Declare"); 
            if (!string.IsNullOrEmpty(json_decl))
            {
                JObject jo_decl = (JObject)JsonConvert.DeserializeObject(json_decl);
                try
                {
                    string sql = "delete from list_declaration where code='" + jo_decl.Value<string>("CODE") + "'"; //先删除已经存在的预制报关单号
                    DBMgr.ExecuteNonQuery(sql);
                    //添加到报关单表
                    sql = @"insert into list_declaration(id
                                , code, codetype, ordercode, declarationcode, unitycode, currentcode, customareacode, declway, decltype, portcode, contractno                                
                                , recordcode, channel, conshippercode , conshippername, busiunitcode, busiunitname, repunitcode, repunitname, transmodel, transname                                
                                , voyageno, blno, exemptioncode, tradecode, tradecountrycode, secountrycode, seportcode , seplacecode, goodsnum, packagecode                                
                                , licenseno, goodsgw, goodsnw, tradetermscode, fgcode, freight, fgunitcode, ipcode, insurancepremium, ipunitcode
                                , aecode, additionalexpenses, aeunitcode, specialrelation, priceimpact, paypoyalties, taxrate, taxunitcode, taxunitname, listinfo

                                , remark, isinvalid, ispause, moedit, coedit, mostarttime, moendtime, mostartid, mostartname, moendid
                                , moendname, costarttime, coendtime, costartid, costartname, coendid, coendname, prestarttime, preendtime, prestartid                                
                                , prestartname, preendid, preendname, ckfinishtime, ckid , ckname, repstarttime, repid, repname, relatedtime
                                , relateduserid, relatedusername, rependtime, rependid, rependname, repovertime, repoverid, repovername, conshippernum, busiunitnum                                
                                , repunitnum, isneedclearance, ishaveclearance, isforcelaw, issplit, warehouseno, yardcode, status, sheetnum, presheetnum      
                          
                                , commoditynum, isaccept, modifyflag, checkflag, preedit, wpid, wpname, wptime, cusno, dataconfirm                                
                                , relatedflag, repoverflag, preacctime, preaccuserid, preaccusername, repwayid, customsstatus, prependtime, prependuserid, prependusername                                
                                , totalnw, totalmoney, totalnum, isprint, printtime, printnum, preedituserid, preeditusername, preedittime, listtype                                
                                , formatauto, busitype, associatepedeclno, associatedeclno, declcodesource, declremark, pauseuserid, pauseusername, specialdecl, dataconfirmuserid                                
                                , dataconfirmusername, dataconfirmusertime, mocurrentid, cocurrentid
                                )
                        values (list_declaration_id.nextval
                                ,'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}'
                                ,'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}'
                                ,'{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}'
                                ,'{31}','{32}','{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}'
                                ,'{41}','{42}','{43}','{44}','{45}','{46}','{47}','{48}','{49}','{50}'

                                ,'{51}','{52}','{53}','{54}','{55}',{56},{57},'{58}','{59}','{60}'
                                ,'{61}',{62},{63},'{64}','{65}','{66}','{67}',{68},{69},'{70}'
                                ,'{71}','{72}','{73}',{74},'{75}','{76}',{77},'{78}','{79}',{80}
                                ,'{81}','{82}',{83},'{84}','{85}',{86},'{87}','{88}','{89}','{90}'
                                ,'{91}','{92}','{93}','{94}','{95}','{96}','{97}','{98}','{99}','{100}'

                                ,'{101}','{102}','{103}','{104}','{105}','{106}','{107}',{108},'{109}','{110}'
                                ,'{111}','{112}',{113},'{114}','{115}','{116}','{117}',{118},'{119}','{120}'
                                ,'{121}','{122}','{123}','{124}',{125},'{126}','{127}','{128}',{129},'{130}'
                                ,'{131}','{132}','{133}','{134}','{135}','{136}','{137}','{138}','{139}','{140}'
                                ,'{141}',{142},'{143}','{144}'
                                )";
                    sql = string.Format(sql
                     , jo_decl.Value<string>("CODE"), jo_decl.Value<string>("CODETYPE"), jo_decl.Value<string>("ORDERCODE"), jo_decl.Value<string>("DECLARATIONCODE"), jo_decl.Value<string>("UNITYCODE"), jo_decl.Value<string>("CURRENTCODE"), jo_decl.Value<string>("CUSTOMAREACODE"), jo_decl.Value<string>("DECLWAY"), jo_decl.Value<string>("DECLTYPE"), jo_decl.Value<string>("PORTCODE"), jo_decl.Value<string>("CONTRACTNO")
                                 , jo_decl.Value<string>("RECORDCODE"), jo_decl.Value<string>("CHANNEL"), jo_decl.Value<string>("CONSHIPPERCODE"), jo_decl.Value<string>("CONSHIPPERNAME"), jo_decl.Value<string>("BUSIUNITCODE"), jo_decl.Value<string>("BUSIUNITNAME"), jo_decl.Value<string>("REPUNITCODE"), jo_decl.Value<string>("REPUNITNAME"), jo_decl.Value<string>("TRANSMODEL"), jo_decl.Value<string>("TRANSNAME ")
                                 , jo_decl.Value<string>("VOYAGENO"), jo_decl.Value<string>("BLNO"), jo_decl.Value<string>("EXEMPTIONCODE"), jo_decl.Value<string>("TRADECODE"), jo_decl.Value<string>("TRADECOUNTRYCODE"), jo_decl.Value<string>("SECOUNTRYCODE"), jo_decl.Value<string>("SEPORTCODE"), jo_decl.Value<string>("SEPLACECODE"), jo_decl.Value<string>("GOODSNUM"), jo_decl.Value<string>("PACKAGECODE")
                                 , jo_decl.Value<string>("LICENSENO"), jo_decl.Value<string>("GOODSGW"), jo_decl.Value<string>("GOODSNW"), jo_decl.Value<string>("TRADETERMSCODE"), jo_decl.Value<string>("FGCODE"), jo_decl.Value<string>("FREIGHT"), jo_decl.Value<string>("FGUNITCODE"), jo_decl.Value<string>("IPCODE"), jo_decl.Value<string>("INSURANCEPREMIUM"), jo_decl.Value<string>("IPUNITCODE")
                                 , jo_decl.Value<string>("AECODE"), jo_decl.Value<string>("ADDITIONALEXPENSES"), jo_decl.Value<string>("AEUNITCODE"), jo_decl.Value<string>("SPECIALRELATION"), jo_decl.Value<string>("PRICEIMPACT"), jo_decl.Value<string>("PAYPOYALTIES"), jo_decl.Value<string>("TAXRATE"), jo_decl.Value<string>("TAXUNITCODE"), jo_decl.Value<string>("TAXUNITNAME"), jo_decl.Value<string>("LISTINFO")

                                 , jo_decl.Value<string>("REMARK"), jo_decl.Value<string>("ISINVALID"), jo_decl.Value<string>("ISPAUSE"), jo_decl.Value<string>("MOEDIT"), jo_decl.Value<string>("COEDIT"), "TO_DATE('" + jo_decl.Value<string>("MOSTARTTIME") + "','yyyy-MM-dd HH24:mi:ss')", "TO_DATE('" + jo_decl.Value<string>("MOENDTIME") + "','yyyy-MM-dd HH24:mi:ss')", jo_decl.Value<string>("MOSTARTID"), jo_decl.Value<string>("MOSTARTNAME"), jo_decl.Value<string>("MOENDID")
                                 , jo_decl.Value<string>("MOENDNAME"), "TO_DATE('" + jo_decl.Value<string>("COSTARTTIME") + "','yyyy-MM-dd HH24:mi:ss')", "TO_DATE('" + jo_decl.Value<string>("COENDTIME") + "','yyyy-MM-dd HH24:mi:ss')", jo_decl.Value<string>("COSTARTID"), jo_decl.Value<string>("COSTARTNAME"), jo_decl.Value<string>("COENDID"), jo_decl.Value<string>("COENDNAME"), "TO_DATE('" + jo_decl.Value<string>("PRESTARTTIME") + "','yyyy-MM-dd HH24:mi:ss')", "TO_DATE('" + jo_decl.Value<string>("PREENDTIME") + "','yyyy-MM-dd HH24:mi:ss')", jo_decl.Value<string>("PRESTARTID")
                                 , jo_decl.Value<string>("PRESTARTNAME"), jo_decl.Value<string>("PREENDID"), jo_decl.Value<string>("PREENDNAME"), "TO_DATE('" + jo_decl.Value<string>("CKFINISHTIME") + "','yyyy-MM-dd HH24:mi:ss')", jo_decl.Value<string>("CKID"), jo_decl.Value<string>("CKNAME"), "TO_DATE('" + jo_decl.Value<string>("REPSTARTTIME") + "','yyyy-MM-dd HH24:mi:ss')", jo_decl.Value<string>("REPID"), jo_decl.Value<string>("REPNAME"), "TO_DATE('" + jo_decl.Value<string>("RELATEDTIME") + "','yyyy-MM-dd HH24:mi:ss')"
                                 , jo_decl.Value<string>("RELATEDUSERID"), jo_decl.Value<string>("RELATEDUSERNAME"), "TO_DATE('" + jo_decl.Value<string>("REPENDTIME") + "','yyyy-MM-dd HH24:mi:ss')", jo_decl.Value<string>("REPENDID"), jo_decl.Value<string>("REPENDNAME"), "TO_DATE('" + jo_decl.Value<string>("REPOVERTIME") + "','yyyy-MM-dd HH24:mi:ss')", jo_decl.Value<string>("REPOVERID"), jo_decl.Value<string>("REPOVERNAME"), jo_decl.Value<string>("CONSHIPPERNUM"), jo_decl.Value<string>("BUSIUNITNUM")
                                 , jo_decl.Value<string>("REPUNITNUM"), jo_decl.Value<string>("ISNEEDCLEARANCE"), jo_decl.Value<string>("ISHAVECLEARANCE"), jo_decl.Value<string>("ISFORCELAW"), jo_decl.Value<string>("ISSPLIT"), jo_decl.Value<string>("WAREHOUSENO"), jo_decl.Value<string>("YARDCODE"), jo_decl.Value<string>("STATUS"), jo_decl.Value<string>("SHEETNUM"), jo_decl.Value<string>("PRESHEETNUM")

                                 , jo_decl.Value<string>("COMMODITYNUM"), jo_decl.Value<string>("ISACCEPT"), jo_decl.Value<string>("MODIFYFLAG"), jo_decl.Value<string>("CHECKFLAG"), jo_decl.Value<string>("PREEDIT"), jo_decl.Value<string>("WPID"), jo_decl.Value<string>("WPNAME"), "TO_DATE('" + jo_decl.Value<string>("WPTIME") + "','yyyy-MM-dd HH24:mi:ss')", jo_decl.Value<string>("CUSNO"), jo_decl.Value<string>("DATACONFIRM")
                                 , jo_decl.Value<string>("RELATEDFLAG"), jo_decl.Value<string>("REPOVERFLAG"), "TO_DATE('" + jo_decl.Value<string>("PREACCTIME") + "','yyyy-MM-dd HH24:mi:ss')", jo_decl.Value<string>("PREACCUSERID"), jo_decl.Value<string>("PREACCUSERNAME"), jo_decl.Value<string>("REPWAYID"), jo_decl.Value<string>("CUSTOMSSTATUS"), "TO_DATE('" + jo_decl.Value<string>("PREPENDTIME") + "','yyyy-MM-dd HH24:mi:ss')", jo_decl.Value<string>("PREPENDUSERID"), jo_decl.Value<string>("PREPENDUSERNAME")
                                 , jo_decl.Value<string>("TOTALNW"), jo_decl.Value<string>("TOTALMONEY"), jo_decl.Value<string>("TOTALNUM"), jo_decl.Value<string>("ISPRINT"), "TO_DATE('" + jo_decl.Value<string>("PRINTTIME") + "','yyyy-MM-dd HH24:mi:ss')", jo_decl.Value<string>("PRINTNUM"), jo_decl.Value<string>("PREEDITUSERID"), jo_decl.Value<string>("PREEDITUSERNAME"), "TO_DATE('" + jo_decl.Value<string>("PREEDITTIME") + "','yyyy-MM-dd HH24:mi:ss')", jo_decl.Value<string>("LISTTYPE")
                                 , jo_decl.Value<string>("FORMATAUTO"), jo_decl.Value<string>("BUSITYPE"), jo_decl.Value<string>("ASSOCIATEPEDECLNO"), jo_decl.Value<string>("ASSOCIATEDECLNO"), jo_decl.Value<string>("DECLCODESOURCE"), jo_decl.Value<string>("DECLREMARK"), jo_decl.Value<string>("PAUSEUSERID"), jo_decl.Value<string>("PAUSEUSERNAME"), jo_decl.Value<string>("SPECIALDECL"), jo_decl.Value<string>("DATACONFIRMUSERID")
                                 , jo_decl.Value<string>("DATACONFIRMUSERNAME"), "TO_DATE('" + jo_decl.Value<string>("DATACONFIRMUSERTIME") + "','yyyy-MM-dd HH24:mi:ss')", jo_decl.Value<string>("MOCURRENTID"), jo_decl.Value<string>("COCURRENTID")
                                 );
                    DBMgr.ExecuteNonQuery(sql);
                }
                catch (Exception ex)
                {
                    db.ListRightPush("Redis_Declare", json_decl);
                }
            }
        }
    }
}
