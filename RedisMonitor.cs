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
                string json_decl = db.ListLeftPop("Redis_Declare");//获取redis队列中第一份的报关单
                if (!string.IsNullOrEmpty(json_decl))
                {
                    JObject jo_decl = (JObject)JsonConvert.DeserializeObject(json_decl);
                    //先删除已经存在的预制报关单号
                    try
                    {
                        string sql = "delete from list_declaration where code='" + jo_decl.Value<string>("CODE") + "'";
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

                                ,'{51}','{52}','{53}','{54}','{55}','{56}','{57}','{58}','{59}','{60}'
                                ,'{61}','{62}','{63}','{64}','{65}','{66}','{67}','{68}','{69}','{70}'
                                ,'{71}','{72}','{73}','{74}','{75}','{76}','{77}','{78}','{79}','{80}'
                                ,'{81}','{82}','{83}','{84}','{85}','{86}','{87}','{88}','{89}','{90}'
                                ,'{91}','{92}','{93}','{94}','{95}','{96}','{97}','{98}','{99}','{100}'

                                ,'{101}','{102}','{103}','{104}','{105}','{106}','{107}','{108}','{109}','{110}'
                                ,'{111}','{112}','{113}','{114}','{115}','{116}','{117}','{118}','{119}','{120}'
                                ,'{121}','{122}','{123}','{124}','{125}','{126}','{127}','{128}','{129}','{130}'
                                ,'{131}','{132}','{133}','{134}','{135}','{136}','{137}','{138}','{139}','{140}'
                                ,'{141}','{142}','{143}','{144}'
                                )";
                        sql = string.Format(sql
                                            , jo_decl.Value<string>("NAME"), jo_decl.Value<string>("code"), jo_decl.Value<string>("codetype"), jo_decl.Value<string>("ordercode"), jo_decl.Value<string>("declarationcode")
                                            , jo_decl.Value<string>("currentcode"), jo_decl.Value<string>("customareacode"), jo_decl.Value<string>("declway"), jo_decl.Value<string>("decltype"), jo_decl.Value<string>("portcode"), jo_decl.Value<string>("contractno"), jo_decl.Value<string>("unitycode")
                                            , jo_decl.Value<string>("recordcode"), jo_decl.Value<string>("channel"), jo_decl.Value<string>("conshippercode"), jo_decl.Value<string>("conshippername"), jo_decl.Value<string>("busiunitcode"), jo_decl.Value<string>("busiunitname"), jo_decl.Value<string>("repunitcode"), jo_decl.Value<string>("repunitname"), jo_decl.Value<string>("transmodel"), jo_decl.Value<string>("transname ")
                                            , jo_decl.Value<string>("voyageno"), jo_decl.Value<string>("blno"), jo_decl.Value<string>("exemptioncode"), jo_decl.Value<string>("tradecode"), jo_decl.Value<string>("tradecountrycode"), jo_decl.Value<string>("secountrycode"), jo_decl.Value<string>("seportcode"), jo_decl.Value<string>("seplacecode"), jo_decl.Value<string>("goodsnum"), jo_decl.Value<string>("packagecode")
                                            , jo_decl.Value<string>("licenseno"), jo_decl.Value<string>("goodsgw"), jo_decl.Value<string>("goodsnw"), jo_decl.Value<string>("tradetermscode"), jo_decl.Value<string>("fgcode"), jo_decl.Value<string>("freight"), jo_decl.Value<string>("fgunitcode"), jo_decl.Value<string>("ipcode"), jo_decl.Value<string>("insurancepremium"), jo_decl.Value<string>("ipunitcode")
                                            , jo_decl.Value<string>("aecode"), jo_decl.Value<string>("additionalexpenses"), jo_decl.Value<string>("aeunitcode"), jo_decl.Value<string>("specialrelation"), jo_decl.Value<string>("priceimpact"), jo_decl.Value<string>("paypoyalties"), jo_decl.Value<string>("taxrate"), jo_decl.Value<string>("taxunitcode"), jo_decl.Value<string>("taxunitname"), jo_decl.Value<string>("listinfo")

                                            , jo_decl.Value<string>("remark"), jo_decl.Value<string>("isinvalid"), jo_decl.Value<string>("ispause"), jo_decl.Value<string>("moedit"), jo_decl.Value<string>("coedit"), jo_decl.Value<string>("mostarttime"), jo_decl.Value<string>("moendtime"), jo_decl.Value<string>("mostartid"), jo_decl.Value<string>("mostartname"), jo_decl.Value<string>("moendid")
                                            , jo_decl.Value<string>("moendname"), jo_decl.Value<string>("costarttime"), jo_decl.Value<string>("coendtime"), jo_decl.Value<string>("costartid"), jo_decl.Value<string>("costartname"), jo_decl.Value<string>("coendid"), jo_decl.Value<string>("coendname"), jo_decl.Value<string>("prestarttime"), jo_decl.Value<string>("preendtime"), jo_decl.Value<string>("prestartid")
                                            , jo_decl.Value<string>("prestartname"), jo_decl.Value<string>("preendid"), jo_decl.Value<string>("preendname"), jo_decl.Value<string>("ckfinishtime"), jo_decl.Value<string>("ckid"), jo_decl.Value<string>("ckname"), jo_decl.Value<string>("repstarttime"), jo_decl.Value<string>("repid"), jo_decl.Value<string>("repname"), jo_decl.Value<string>("relatedtime")
                                            , jo_decl.Value<string>("relateduserid"), jo_decl.Value<string>("relatedusername"), jo_decl.Value<string>("rependtime"), jo_decl.Value<string>("rependid"), jo_decl.Value<string>("rependname"), jo_decl.Value<string>("repovertime"), jo_decl.Value<string>("repoverid"), jo_decl.Value<string>("repovername"), jo_decl.Value<string>("conshippernum"), jo_decl.Value<string>("busiunitnum")
                                            , jo_decl.Value<string>("repunitnum"), jo_decl.Value<string>("isneedclearance"), jo_decl.Value<string>("ishaveclearance"), jo_decl.Value<string>("isforcelaw"), jo_decl.Value<string>("issplit"), jo_decl.Value<string>("warehouseno"), jo_decl.Value<string>("yardcode"), jo_decl.Value<string>("status"), jo_decl.Value<string>("sheetnum"), jo_decl.Value<string>("presheetnum")

                                            , jo_decl.Value<string>("commoditynum"), jo_decl.Value<string>("isaccept"), jo_decl.Value<string>("modifyflag"), jo_decl.Value<string>("checkflag"), jo_decl.Value<string>("preedit"), jo_decl.Value<string>("wpid"), jo_decl.Value<string>("wpname"), jo_decl.Value<string>("wptime"), jo_decl.Value<string>("cusno"), jo_decl.Value<string>("dataconfirm")
                                            , jo_decl.Value<string>("relatedflag"), jo_decl.Value<string>("repoverflag"), jo_decl.Value<string>("preacctime"), jo_decl.Value<string>("preaccuserid"), jo_decl.Value<string>("preaccusername"), jo_decl.Value<string>("repwayid"), jo_decl.Value<string>("customsstatus"), jo_decl.Value<string>("prependtime"), jo_decl.Value<string>("prependuserid"), jo_decl.Value<string>("prependusername")
                                            , jo_decl.Value<string>("totalnw"), jo_decl.Value<string>("totalmoney"), jo_decl.Value<string>("totalnum"), jo_decl.Value<string>("isprint"), jo_decl.Value<string>("printtime"), jo_decl.Value<string>("printnum"), jo_decl.Value<string>("preedituserid"), jo_decl.Value<string>("preeditusername"), jo_decl.Value<string>("preedittime"), jo_decl.Value<string>("listtype")
                                            , jo_decl.Value<string>("formatauto"), jo_decl.Value<string>("busitype"), jo_decl.Value<string>("associatepedeclno"), jo_decl.Value<string>("associatedeclno"), jo_decl.Value<string>("declcodesource"), jo_decl.Value<string>("declremark"), jo_decl.Value<string>("pauseuserid"), jo_decl.Value<string>("pauseusername"), jo_decl.Value<string>("specialdecl"), jo_decl.Value<string>("dataconfirmuserid")
                                            , jo_decl.Value<string>("dataconfirmusername"), jo_decl.Value<string>("dataconfirmusertime"), jo_decl.Value<string>("mocurrentid"), jo_decl.Value<string>("cocurrentid")
                                            );

                        DBMgr.ExecuteNonQuery(sql);
                    }
                    catch (Exception ex)
                    {
                        db.ListRightPush("Redis_Declare", json_decl);                    
                    }
                }
                working = false;
            }
        }
    }
}
