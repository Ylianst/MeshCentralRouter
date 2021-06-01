/*
Copyright 2009-2021 Intel Corporation

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public class Translate
    {
        // *** TRANSLATION TABLE START ***
        static private Dictionary<string, Dictionary<string, string>> translationTable = new Dictionary<string, Dictionary<string, string>>() {
        {
            "Sort by G&roup",
            new Dictionary<string, string>() {
                {"es","Hlig yb T&ilfk"}
            }
        },
        {
            "Tunnelling Data",
            new Dictionary<string, string>() {
                {"es","Gfmmvoormt Wzgz"}
            }
        },
        {
            "MeshCentral Router",
            new Dictionary<string, string>() {
                {"es","NvhsXvmgizo Ilfgvi"}
            }
        },
        {
            "SMS",
            new Dictionary<string, string>() {
                {"es","HNH"}
            }
        },
        {
            "Sort by &Name",
            new Dictionary<string, string>() {
                {"es","Hlig yb &Mznv"}
            }
        },
        {
            "Protocol",
            new Dictionary<string, string>() {
                {"es","Kilglxlo"}
            }
        },
        {
            "Password",
            new Dictionary<string, string>() {
                {"es","Kzhhdliw"}
            }
        },
        {
            "Agent",
            new Dictionary<string, string>() {
                {"es","Ztvmg"}
            }
        },
        {
            "Settings",
            new Dictionary<string, string>() {
                {"es","Hvggrmth"}
            }
        },
        {
            "&Info...",
            new Dictionary<string, string>() {
                {"es","&Rmul..."}
            }
        },
        {
            "Email",
            new Dictionary<string, string>() {
                {"es","Vnzro"}
            }
        },
        {
            "Unable to connect",
            new Dictionary<string, string>() {
                {"es","Fmzyov gl xlmmvxg"}
            }
        },
        {
            "UDP",
            new Dictionary<string, string>() {
                {"es","FWK"}
            }
        },
        {
            "Remote IP",
            new Dictionary<string, string>() {
                {"es","Ivnlgv RK"}
            }
        },
        {
            "TCP",
            new Dictionary<string, string>() {
                {"es","GXK"}
            }
        },
        {
            "Connection",
            new Dictionary<string, string>() {
                {"es","Xlmmvxgrlm"}
            }
        },
        {
            "PuTTY SSH client",
            new Dictionary<string, string>() {
                {"es","KfGGB HHS xorvmg"}
            }
        },
        {
            "MeshCentral",
            new Dictionary<string, string>() {
                {"es","NvhsXvmgizo"}
            }
        },
        {
            "MeshCentral Router allows mapping of TCP and UDP ports on this computer to any computer in your MeshCentral server account. Start by logging into your account.",
            new Dictionary<string, string>() {
                {"es","NvhsXvmgizo Ilfgvi zooldh nzkkrmt lu GXK zmw FWK kligh lm gsrh xlnkfgvi gl zmb xlnkfgvi rm blfi NvhsXvmgizo hvievi zxxlfmg. Hgzig yb olttrmt rmgl blfi zxxlfmg."}
            }
        },
        {
            "v",
            new Dictionary<string, string>() {
                {"es","e"}
            }
        },
        {
            "Invalid username or password",
            new Dictionary<string, string>() {
                {"es","Rmezorw fhvimznv li kzhhdliw"}
            }
        },
        {
            "Email sent",
            new Dictionary<string, string>() {
                {"es","Vnzro hvmg"}
            }
        },
        {
            "Cancel Auto-Close",
            new Dictionary<string, string>() {
                {"es","Xzmxvo Zfgl-Xolhv"}
            }
        },
        {
            "&Open Mappings...",
            new Dictionary<string, string>() {
                {"es","&Lkvm Nzkkrmth..."}
            }
        },
        {
            "Server information",
            new Dictionary<string, string>() {
                {"es","Hvievi rmulinzgrlm"}
            }
        },
        {
            "Show on system tray",
            new Dictionary<string, string>() {
                {"es","Hsld lm hbhgvn gizb"}
            }
        },
        {
            "Install...",
            new Dictionary<string, string>() {
                {"es","Rmhgzoo..."}
            }
        },
        {
            "WinSCP client",
            new Dictionary<string, string>() {
                {"es","DrmHXK xorvmg"}
            }
        },
        {
            "0%",
            new Dictionary<string, string>() {

            }
        },
        {
            "Token",
            new Dictionary<string, string>() {
                {"es","Glpvm"}
            }
        },
        {
            "Back",
            new Dictionary<string, string>() {
                {"es","Yzxp"}
            }
        },
        {
            "Remote Desktop",
            new Dictionary<string, string>() {
                {"es","Ivnlgv Wvhpglk"}
            }
        },
        {
            "Search",
            new Dictionary<string, string>() {
                {"es","Hvzixs"}
            }
        },
        {
            "MeshCentral Router Installation",
            new Dictionary<string, string>() {
                {"es","NvhsXvmgizo Ilfgvi Rmhgzoozgrlm"}
            }
        },
        {
            "No Devices",
            new Dictionary<string, string>() {
                {"es","Ml Wverxvh"}
            }
        },
        {
            "Device Status",
            new Dictionary<string, string>() {
                {"es","Wverxv Hgzgfh"}
            }
        },
        {
            "Bind local port to all network interfaces",
            new Dictionary<string, string>() {
                {"es","Yrmw olxzo klig gl zoo mvgdlip rmgviuzxvh"}
            }
        },
        {
            "Installation",
            new Dictionary<string, string>() {
                {"es","Rmhgzoozgrlm"}
            }
        },
        {
            "v0.8.0",
            new Dictionary<string, string>() {
                {"es","e0.8.0"}
            }
        },
        {
            "Add &Relay Map...",
            new Dictionary<string, string>() {
                {"es","Zww &Ivozb Nzk..."}
            }
        },
        {
            "SSH",
            new Dictionary<string, string>() {
                {"es","HHS"}
            }
        },
        {
            "Compressed Network Traffic",
            new Dictionary<string, string>() {
                {"es","Xlnkivhhvw Mvgdlip Gizuurx"}
            }
        },
        {
            "View Certificate Details...",
            new Dictionary<string, string>() {
                {"es","Ervd Xvigrurxzgv Wvgzroh..."}
            }
        },
        {
            " MeshCentral Router",
            new Dictionary<string, string>() {
                {"es"," NvhsXvmgizo Ilfgvi"}
            }
        },
        {
            "CIRA",
            new Dictionary<string, string>() {
                {"es","XRIZ"}
            }
        },
        {
            "State",
            new Dictionary<string, string>() {
                {"es","Hgzgv"}
            }
        },
        {
            "Ask Consent",
            new Dictionary<string, string>() {
                {"es","Zhp Xlmhvmg"}
            }
        },
        {
            "AMT",
            new Dictionary<string, string>() {
                {"es","ZNG"}
            }
        },
        {
            "Add Map...",
            new Dictionary<string, string>() {
                {"es","Zww Nzk..."}
            }
        },
        {
            "Don't ask for {0} days.",
            new Dictionary<string, string>() {
                {"es","Wlm'g zhp uli {0} wzbh."}
            }
        },
        {
            "Use Alternate Port...",
            new Dictionary<string, string>() {
                {"es","Fhv Zogvimzgv Klig..."}
            }
        },
        {
            "Send token to registered phone number?",
            new Dictionary<string, string>() {
                {"es","Hvmw glpvm gl ivtrhgvivw kslmv mfnyvi?"}
            }
        },
        {
            "Unable to bind to local port",
            new Dictionary<string, string>() {
                {"es","Fmzyov gl yrmw gl olxzo klig"}
            }
        },
        {
            "Open...",
            new Dictionary<string, string>() {
                {"es","Lkvm..."}
            }
        },
        {
            "Application",
            new Dictionary<string, string>() {
                {"es","Zkkorxzgrlm"}
            }
        },
        {
            "Connect",
            new Dictionary<string, string>() {
                {"es","Xlmmvxg"}
            }
        },
        {
            "Error Message",
            new Dictionary<string, string>() {
                {"es","Viili Nvhhztv"}
            }
        },
        {
            "Changing language will close this tool. Are you sure?",
            new Dictionary<string, string>() {
                {"es","Xszmtrmt ozmtfztv droo xolhv gsrh gllo. Ziv blf hfiv?"}
            }
        },
        {
            "Remote Desktop...",
            new Dictionary<string, string>() {
                {"es","Ivnlgv Wvhpglk..."}
            }
        },
        {
            "ServerName",
            new Dictionary<string, string>() {
                {"es","HvieviMznv"}
            }
        },
        {
            "Swap Mouse Buttons",
            new Dictionary<string, string>() {
                {"es","Hdzk Nlfhv Yfgglmh"}
            }
        },
        {
            ", 1 connection.",
            new Dictionary<string, string>() {
                {"es",", 1 xlmmvxgrlm."}
            }
        },
        {
            "Port Mapping",
            new Dictionary<string, string>() {
                {"es","Klig Nzkkrmt"}
            }
        },
        {
            "Languages",
            new Dictionary<string, string>() {
                {"es","Ozmtfztvh"}
            }
        },
        {
            "Port Mapping Help",
            new Dictionary<string, string>() {
                {"es","Klig Nzkkrmt Svok"}
            }
        },
        {
            "Open Source, Apache 2.0 License",
            new Dictionary<string, string>() {
                {"es","Lkvm Hlfixv, Zkzxsv 2.0 Orxvmhv"}
            }
        },
        {
            "OpenSSH",
            new Dictionary<string, string>() {
                {"es","LkvmHHS"}
            }
        },
        {
            "Open Web Site",
            new Dictionary<string, string>() {
                {"es","Lkvm Dvy Hrgv"}
            }
        },
        {
            "Stopped",
            new Dictionary<string, string>() {
                {"es","Hglkkvw"}
            }
        },
        {
            "Mappings",
            new Dictionary<string, string>() {
                {"es","Nzkkrmth"}
            }
        },
        {
            "Relay Device",
            new Dictionary<string, string>() {
                {"es","Ivozb Wverxv"}
            }
        },
        {
            "0 Bytes",
            new Dictionary<string, string>() {
                {"es","0 Ybgvh"}
            }
        },
        {
            "Certificate",
            new Dictionary<string, string>() {
                {"es","Xvigrurxzgv"}
            }
        },
        {
            "&Save Mappings...",
            new Dictionary<string, string>() {
                {"es","&Hzev Nzkkrmth..."}
            }
        },
        {
            "Stats...",
            new Dictionary<string, string>() {
                {"es","Hgzgh..."}
            }
        },
        {
            "Username",
            new Dictionary<string, string>() {
                {"es","Fhvimznv"}
            }
        },
        {
            ", {0} connections.",
            new Dictionary<string, string>() {
                {"es",", {0} xlmmvxgrlmh."}
            }
        },
        {
            "Set RDP port...",
            new Dictionary<string, string>() {
                {"es","Hvg IWK klig..."}
            }
        },
        {
            "&Delete",
            new Dictionary<string, string>() {
                {"es","&Wvovgv"}
            }
        },
        {
            "E&xit",
            new Dictionary<string, string>() {
                {"es","V&crg"}
            }
        },
        {
            "Quality",
            new Dictionary<string, string>() {
                {"es","Jfzorgb"}
            }
        },
        {
            "Email verification required",
            new Dictionary<string, string>() {
                {"es","Vnzro evirurxzgrlm ivjfrivw"}
            }
        },
        {
            "Frame rate",
            new Dictionary<string, string>() {
                {"es","Uiznv izgv"}
            }
        },
        {
            "Relay Mapping",
            new Dictionary<string, string>() {
                {"es","Ivozb Nzkkrmt"}
            }
        },
        {
            "Server",
            new Dictionary<string, string>() {
                {"es","Hvievi"}
            }
        },
        {
            "Remote Files...",
            new Dictionary<string, string>() {
                {"es","Ivnlgv Urovh..."}
            }
        },
        {
            "R&efresh",
            new Dictionary<string, string>() {
                {"es","I&vuivhs"}
            }
        },
        {
            "Add &Map...",
            new Dictionary<string, string>() {
                {"es","Zww &Nzk..."}
            }
        },
        {
            "RDP",
            new Dictionary<string, string>() {
                {"es","IWK"}
            }
        },
        {
            "Cancel",
            new Dictionary<string, string>() {
                {"es","Xzmxvo"}
            }
        },
        {
            "Remember this certificate",
            new Dictionary<string, string>() {
                {"es","Ivnvnyvi gsrh xvigrurxzgv"}
            }
        },
        {
            "(Individual Devices)",
            new Dictionary<string, string>() {
                {"es","(Rmwrerwfzo Wverxvh)"}
            }
        },
        {
            "Alternative Port",
            new Dictionary<string, string>() {
                {"es","Zogvimzgrev Klig"}
            }
        },
        {
            "Remote Files",
            new Dictionary<string, string>() {
                {"es","Ivnlgv Urovh"}
            }
        },
        {
            "Incoming Bytes",
            new Dictionary<string, string>() {
                {"es","Rmxlnrmt Ybgvh"}
            }
        },
        {
            "Relay",
            new Dictionary<string, string>() {
                {"es","Ivozb"}
            }
        },
        {
            "statusStrip1",
            new Dictionary<string, string>() {
                {"es","hgzgfhHgirk1"}
            }
        },
        {
            "SMS sent",
            new Dictionary<string, string>() {
                {"es","HNH hvmg"}
            }
        },
        {
            "Application Name",
            new Dictionary<string, string>() {
                {"es","Zkkorxzgrlm Mznv"}
            }
        },
        {
            "Remote Desktop Stats",
            new Dictionary<string, string>() {
                {"es","Ivnlgv Wvhpglk Hgzgh"}
            }
        },
        {
            "≡",
            new Dictionary<string, string>() {

            }
        },
        {
            "Click ok to register MeshCentral Router on your system as the handler for the \"mcrouter://\" protocol. This will allow the MeshCentral web site to launch this application when needed.",
            new Dictionary<string, string>() {
                {"es","Xorxp lp gl ivtrhgvi NvhsXvmgizo Ilfgvi lm blfi hbhgvn zh gsv szmwovi uli gsv \"nxilfgvi://\" kilglxlo. Gsrh droo zoold gsv NvhsXvmgizo dvy hrgv gl ozfmxs gsrh zkkorxzgrlm dsvm mvvwvw."}
            }
        },
        {
            "Size",
            new Dictionary<string, string>() {
                {"es","Hrav"}
            }
        },
        {
            "Remote Port",
            new Dictionary<string, string>() {
                {"es","Ivnlgv Klig"}
            }
        },
        {
            "No Port Mappings\r\n\r\nClick \"Add\" to get started.",
            new Dictionary<string, string>() {
                {"es","Ml Klig Nzkkrmth\r\n\r\nXorxp \"Zww\" gl tvg hgzigvw."}
            }
        },
        {
            "Ctrl-Alt-Del",
            new Dictionary<string, string>() {
                {"es","Xgio-Zog-Wvo"}
            }
        },
        {
            "HTTP",
            new Dictionary<string, string>() {
                {"es","SGGK"}
            }
        },
        {
            "Site",
            new Dictionary<string, string>() {
                {"es","Hrgv"}
            }
        },
        {
            "Two-factor Authentication",
            new Dictionary<string, string>() {
                {"es","Gdl-uzxgli Zfgsvmgrxzgrlm"}
            }
        },
        {
            "Application Launch",
            new Dictionary<string, string>() {
                {"es","Zkkorxzgrlm Ozfmxs"}
            }
        },
        {
            "Add Relay Map...",
            new Dictionary<string, string>() {
                {"es","Zww Ivozb Nzk..."}
            }
        },
        {
            "Mapping Settings",
            new Dictionary<string, string>() {
                {"es","Nzkkrmt Hvggrmth"}
            }
        },
        {
            "S&ettings...",
            new Dictionary<string, string>() {
                {"es","H&vggrmth..."}
            }
        },
        {
            "Enter the RDP port of the remote computer, the default RDP port is 3389.",
            new Dictionary<string, string>() {
                {"es","Vmgvi gsv IWK klig lu gsv ivnlgv xlnkfgvi, gsv wvuzfog IWK klig rh 3389."}
            }
        },
        {
            "Devices",
            new Dictionary<string, string>() {
                {"es","Wverxvh"}
            }
        },
        {
            "Starting...",
            new Dictionary<string, string>() {
                {"es","Hgzigrmt..."}
            }
        },
        {
            "Local",
            new Dictionary<string, string>() {
                {"es","Olxzo"}
            }
        },
        {
            "Remote Desktop Settings",
            new Dictionary<string, string>() {
                {"es","Ivnlgv Wvhpglk Hvggrmth"}
            }
        },
        {
            "Stopped.",
            new Dictionary<string, string>() {
                {"es","Hglkkvw."}
            }
        },
        {
            "Outgoing Compression",
            new Dictionary<string, string>() {
                {"es","Lfgtlrmt Xlnkivhhrlm"}
            }
        },
        {
            "Application Link",
            new Dictionary<string, string>() {
                {"es","Zkkorxzgrlm Ormp"}
            }
        },
        {
            "---",
            new Dictionary<string, string>() {

            }
        },
        {
            "This server presented a un-trusted certificate.  This may indicate that this is not the correct server or that the server does not have a valid certificate. It is not recommanded, but you can press the ignore button to continue connection to this server.",
            new Dictionary<string, string>() {
                {"es","Gsrh hvievi kivhvmgvw z fm-gifhgvw xvigrurxzgv.  Gsrh nzb rmwrxzgv gszg gsrh rh mlg gsv xliivxg hvievi li gszg gsv hvievi wlvh mlg szev z ezorw xvigrurxzgv. Rg rh mlg ivxlnnzmwvw, yfg blf xzm kivhh gsv rtmliv yfgglm gl xlmgrmfv xlmmvxgrlm gl gsrh hvievi."}
            }
        },
        {
            "MQTT",
            new Dictionary<string, string>() {
                {"es","NJGG"}
            }
        },
        {
            "127.0.0.1",
            new Dictionary<string, string>() {

            }
        },
        {
            "SSH Username",
            new Dictionary<string, string>() {
                {"es","HHS Fhvimznv"}
            }
        },
        {
            "Next",
            new Dictionary<string, string>() {
                {"es","Mvcg"}
            }
        },
        {
            "Enter the second factor authentication token.",
            new Dictionary<string, string>() {
                {"es","Vmgvi gsv hvxlmw uzxgli zfgsvmgrxzgrlm glpvm."}
            }
        },
        {
            "No Search Results",
            new Dictionary<string, string>() {
                {"es","Ml Hvzixs Ivhfogh"}
            }
        },
        {
            "Stats",
            new Dictionary<string, string>() {
                {"es","Hgzgh"}
            }
        },
        {
            "Use Remote Keyboard Map",
            new Dictionary<string, string>() {
                {"es","Fhv Ivnlgv Pvbylziw Nzk"}
            }
        },
        {
            "Path",
            new Dictionary<string, string>() {
                {"es","Kzgs"}
            }
        },
        {
            "Send token to registered email address?",
            new Dictionary<string, string>() {
                {"es","Hvmw glpvm gl ivtrhgvivw vnzro zwwivhh?"}
            }
        },
        {
            "Local Port",
            new Dictionary<string, string>() {
                {"es","Olxzo Klig"}
            }
        },
        {
            "label1",
            new Dictionary<string, string>() {
                {"es","ozyvo1"}
            }
        },
        {
            "Scaling",
            new Dictionary<string, string>() {
                {"es","Hxzormt"}
            }
        },
        {
            "SCP",
            new Dictionary<string, string>() {
                {"es","HXK"}
            }
        },
        {
            "Remote desktop quality, scaling and frame rate settings. These can be adjusted depending on the quality of the network connection.",
            new Dictionary<string, string>() {
                {"es","Ivnlgv wvhpglk jfzorgb, hxzormt zmw uiznv izgv hvggrmth. Gsvhv xzm yv zwqfhgvw wvkvmwrmt lm gsv jfzorgb lu gsv mvgdlip xlmmvxgrlm."}
            }
        },
        {
            "ComputerName",
            new Dictionary<string, string>() {
                {"es","XlnkfgviMznv"}
            }
        },
        {
            "Offline",
            new Dictionary<string, string>() {
                {"es","Luuormv"}
            }
        },
        {
            "Log out",
            new Dictionary<string, string>() {
                {"es","Olt lfg"}
            }
        },
        {
            "Show &Offline Devices",
            new Dictionary<string, string>() {
                {"es","Hsld &Luuormv Wverxvh"}
            }
        },
        {
            "&Open...",
            new Dictionary<string, string>() {
                {"es","&Lkvm..."}
            }
        },
        {
            "Privacy Bar",
            new Dictionary<string, string>() {
                {"es","Kirezxb Yzi"}
            }
        },
        {
            "Login",
            new Dictionary<string, string>() {
                {"es","Oltrm"}
            }
        },
        {
            "Ignore",
            new Dictionary<string, string>() {
                {"es","Rtmliv"}
            }
        },
        {
            "RDP Port",
            new Dictionary<string, string>() {
                {"es","IWK Klig"}
            }
        },
        {
            "Routing Stats",
            new Dictionary<string, string>() {
                {"es","Ilfgrmt Hgzgh"}
            }
        },
        {
            "Show &Group Names",
            new Dictionary<string, string>() {
                {"es","Hsld &Tilfk Mznvh"}
            }
        },
        {
            "Remote",
            new Dictionary<string, string>() {
                {"es","Ivnlgv"}
            }
        },
        {
            "Remote Device",
            new Dictionary<string, string>() {
                {"es","Ivnlgv Wverxv"}
            }
        },
        {
            "Remote Desktop Data",
            new Dictionary<string, string>() {
                {"es","Ivnlgv Wvhpglk Wzgz"}
            }
        },
        {
            "Device Group",
            new Dictionary<string, string>() {
                {"es","Wverxv Tilfk"}
            }
        },
        {
            "&Rename",
            new Dictionary<string, string>() {
                {"es","&Ivmznv"}
            }
        },
        {
            "WARNING - Invalid Server Certificate",
            new Dictionary<string, string>() {
                {"es","DZIMRMT - Rmezorw Hvievi Xvigrurxzgv"}
            }
        },
        {
            "Close",
            new Dictionary<string, string>() {
                {"es","Xolhv"}
            }
        },
        {
            "Outgoing Bytes",
            new Dictionary<string, string>() {
                {"es","Lfgtlrmt Ybgvh"}
            }
        },
        {
            "OK",
            new Dictionary<string, string>() {
                {"es","LP"}
            }
        },
        {
            "Routing Status",
            new Dictionary<string, string>() {
                {"es","Ilfgrmt Hgzgfh"}
            }
        },
        {
            "Ask Consent + Bar",
            new Dictionary<string, string>() {
                {"es","Zhp Xlmhvmg + Yzi"}
            }
        },
        {
            "Name",
            new Dictionary<string, string>() {
                {"es","Mznv"}
            }
        },
        {
            "Remove",
            new Dictionary<string, string>() {
                {"es","Ivnlev"}
            }
        },
        {
            "HTTPS",
            new Dictionary<string, string>() {
                {"es","SGGKH"}
            }
        },
        {
            "Incoming Compression",
            new Dictionary<string, string>() {
                {"es","Rmxlnrmt Xlnkivhhrlm"}
            }
        }
        };
        // *** TRANSLATION TABLE END ***

        static public string T(string english)
        {
            string lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            if (lang == "en") return english;
            if (translationTable.ContainsKey(english))
            {
                Dictionary<string, string> translations = translationTable[english];
                if (translations.ContainsKey(lang)) return translations[lang];
            }
            return english;
        }

        static public void TranslateControl(Control control)
        {
            control.Text = T(control.Text);
            foreach (Control c in control.Controls) { TranslateControl(c); }
        }

        static public void TranslateContextMenu(ContextMenuStrip menu)
        {
            menu.Text = T(menu.Text);
            foreach (object i in menu.Items) { if (i.GetType() == typeof(ToolStripMenuItem)) { TranslateToolStripMenuItem((ToolStripMenuItem)i); } }
        }

        static public void TranslateToolStripMenuItem(ToolStripMenuItem menu)
        {
            menu.Text = T(menu.Text);
            foreach (object i in menu.DropDownItems)
            {
                if (i.GetType() == typeof(ToolStripMenuItem))
                {
                    TranslateToolStripMenuItem((ToolStripMenuItem)i);
                }
            }
        }

        static public void TranslateListView(ListView listview)
        {
            listview.Text = T(listview.Text);
            foreach (object c in listview.Columns)
            {
                if (c.GetType() == typeof(ColumnHeader))
                {
                    ((ColumnHeader)c).Text = T(((ColumnHeader)c).Text);
                }
            }
        }


    }
}
