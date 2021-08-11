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
            "Failed to start remote terminal session",
            new Dictionary<string, string>() {
                {"nl","Kan externe terminalsessie niet starten"},
                {"ko","원격 터미널 세션을 시작하지 못했습니다."},
                {"fr","Échec du démarrage de la session de terminal distant"},
                {"it","Impossibile avviare la sessione del terminale remoto"},
                {"zh-chs","无法启动远程终端会话"},
                {"pt-br","Falha ao iniciar sessão de terminal remoto"},
                {"sv","Det gick inte att starta fjärrterminsessionen"},
                {"es","No se pudo iniciar la sesión de terminal remota"},
                {"hi","दूरस्थ टर्मिनल सत्र प्रारंभ करने में विफल"},
                {"de","Fehler beim Starten der Remote-Terminal-Sitzung"}
            }
        },
        {
            "{0} Byte",
            new Dictionary<string, string>() {
                {"ko","{0} 바이트"},
                {"fr","{0} octet"},
                {"zh-chs","{0} 字节"},
                {"hi","{0} बाइट"}
            }
        },
        {
            "Certificate",
            new Dictionary<string, string>() {
                {"nl","Certificaat"},
                {"ko","증명서"},
                {"fr","Certificat"},
                {"it","Certificato"},
                {"zh-chs","证书"},
                {"pt-br","Certificado"},
                {"sv","Certifikat"},
                {"es","Certificado"},
                {"hi","प्रमाणपत्र"},
                {"de","Zertifikat"}
            }
        },
        {
            "Stats...",
            new Dictionary<string, string>() {
                {"nl","Statistieken..."},
                {"ko","통계 ..."},
                {"fr","Statistiques..."},
                {"it","Statistiche..."},
                {"zh-chs","统计..."},
                {"pt-br","Estatísticas..."},
                {"sv","Statistik ..."},
                {"es","Estadísticas ..."},
                {"hi","आँकड़े..."},
                {"de","Statistiken..."}
            }
        },
        {
            "R&efresh",
            new Dictionary<string, string>() {
                {"nl","Vernieuwen"},
                {"ko","새롭게 하다"},
                {"fr","Rafraîchir"},
                {"zh-chs","刷新"},
                {"pt-br","R & efresh"},
                {"sv","R & efresh"},
                {"es","Actualizar"},
                {"hi","ताज़ा करें"},
                {"de","Aktualisierung"}
            }
        },
        {
            "Sort by &Name",
            new Dictionary<string, string>() {
                {"nl","Sorteer op &Naam"},
                {"ko","이름으로 분류하다"},
                {"fr","Trier par nom"},
                {"it","Ordina per nome"},
                {"zh-chs","按名称分类"},
                {"pt-br","Classificar por & nome"},
                {"sv","Sortera efter &Namn"},
                {"es","Ordenar por nombre"},
                {"hi","नाम द्वारा क्रमबद्ध करें"},
                {"de","Nach Name sortieren"}
            }
        },
        {
            "Changing language will close this tool. Are you sure?",
            new Dictionary<string, string>() {
                {"nl","Als u de taal wijzigt, wordt deze tool gesloten.Weet je het zeker?"},
                {"ko","언어를 변경하면이 도구가 닫힙니다. 확실합니까?"},
                {"fr","Le changement de langue fermera cet outil. Êtes-vous sûr?"},
                {"it","La modifica della lingua chiuderà questo strumento. Sei sicuro?"},
                {"zh-chs","更改语言将关闭此工具。你确定吗？"},
                {"pt-br","Alterar o idioma fechará esta ferramenta. Tem certeza?"},
                {"sv","Om du byter språk kommer detta verktyg att stängas. Är du säker?"},
                {"es","El cambio de idioma cerrará esta herramienta. ¿Está seguro?"},
                {"hi","भाषा बदलने से यह टूल बंद हो जाएगा। क्या आपको यकीन है?"},
                {"de","Wenn Sie die Sprache ändern, wird dieses Tool geschlossen. Bist du sicher?"}
            }
        },
        {
            "Device Group",
            new Dictionary<string, string>() {
                {"de","Gerätegruppe"},
                {"hi","डिवाइस समूह"},
                {"fr","Groupe d'appareils"},
                {"zh-cht","裝置群"},
                {"zh-chs","设备组"},
                {"fi","Laiteryhmä"},
                {"tr","Cihaz Grubu"},
                {"cs","Skupina zařízení"},
                {"ja","デバイスグループ"},
                {"es","Grupo de Dispositivos"},
                {"pt","Grupo de dispositivos"},
                {"nl","Apparaat groep"},
                {"pt-br","Grupo de Dispositivos"},
                {"sv","Enhetsgrupp"},
                {"ko","장치 그룹"},
                {"it","Gruppo di dispositivi"},
                {"ru","Группа устройства"}
            }
        },
        {
            "Waiting for user to grant access...",
            new Dictionary<string, string>() {
                {"de","Warten auf den Benutzer, um Zugriff zu gewähren ..."},
                {"hi","उपयोगकर्ता की पहुँच की प्रतीक्षा कर रहा है ..."},
                {"fr","En attente de l'autorisation d'accès par l'utilisateur ..."},
                {"zh-cht","正在等待用戶授予訪問權限..."},
                {"zh-chs","正在等待用户授予访问权限..."},
                {"fi","Odotetaan, että käyttäjä myöntää käyttöoikeuden..."},
                {"tr","Kullanıcının erişim izni vermesi bekleniyor ..."},
                {"cs","Čekání na povolení přístupu uživatelem ..."},
                {"ja","ユーザーがアクセスを許可するのを待っています..."},
                {"es","Esperando a que el usuario otorgue acceso..."},
                {"pt","Esperando que o usuário conceda acesso ..."},
                {"nl","Wachten op toestemming van de gebruiker..."},
                {"pt-br","Esperando que o usuário conceda acesso ..."},
                {"sv","Väntar på att användaren ska ge åtkomst ..."},
                {"ko","사용자가 액세스 권한을 부여하기를 기다리는 중 ..."},
                {"it","In attesa che l'utente conceda l'accesso... "},
                {"ru","Ожидание предоставления доступа пользователем ..."}
            }
        },
        {
            "Remote desktop quality, scaling and frame rate settings. These can be adjusted depending on the quality of the network connection.",
            new Dictionary<string, string>() {
                {"nl","Instellingen voor kwaliteit, schaal en framesnelheid van het externe bureaublad. Deze kunnen worden aangepast afhankelijk van de kwaliteit van de netwerkverbinding."},
                {"ko","원격 데스크톱 품질, 크기 조정 및 프레임 속도 설정. 네트워크 연결 품질에 따라 조정할 수 있습니다."},
                {"fr","Paramètres de qualité, de mise à l'échelle et de fréquence d'images du bureau à distance. Ceux-ci peuvent être ajustés en fonction de la qualité de la connexion réseau."},
                {"it","Impostazioni di qualità, ridimensionamento e frame rate del desktop remoto. Questi possono essere regolati in base alla qualità della connessione di rete."},
                {"zh-chs","远程桌面质量、缩放和帧速率设置。这些可以根据网络连接的质量进行调整。"},
                {"pt-br","Configurações de qualidade, escala e taxa de quadros da área de trabalho remota. Eles podem ser ajustados dependendo da qualidade da conexão de rede."},
                {"sv","Fjärrskrivbords kvalitet, skalning och bildfrekvensinställningar. Dessa kan justeras beroende på nätverksanslutningens kvalitet."},
                {"es","Configuración de la calidad del escritorio remoto, la escala y la velocidad de fotogramas. Estos se pueden ajustar en función de la calidad de la conexión de red."},
                {"hi","दूरस्थ डेस्कटॉप गुणवत्ता, स्केलिंग और फ्रेम दर सेटिंग्स। इन्हें नेटवर्क कनेक्शन की गुणवत्ता के आधार पर समायोजित किया जा सकता है।"},
                {"de","Remote-Desktop-Qualität, Skalierung und Bildrateneinstellungen. Diese können je nach Qualität der Netzwerkverbindung angepasst werden."}
            }
        },
        {
            "Use Alternate Port...",
            new Dictionary<string, string>() {
                {"nl","Alternatieve poort gebruiken..."},
                {"ko","대체 포트 사용 ..."},
                {"fr","Utiliser un autre port..."},
                {"it","Usa porta alternativa..."},
                {"zh-chs","使用备用端口..."},
                {"pt-br","Usar porta alternativa ..."},
                {"sv","Använd alternativ port ..."},
                {"es","Usar Puerto Alternativo ..."},
                {"hi","वैकल्पिक पोर्ट का उपयोग करें..."},
                {"de","Alternativer Port verwenden..."}
            }
        },
        {
            "No Port Mappings\r\n\r\nClick \"Add\" to get started.",
            new Dictionary<string, string>() {
                {"nl","Geen poorttoewijzingen\r\n\r\nKlik op \"Toevoegen\" om te beginnen."},
                {"ko","포트 매핑 없음\r\n\r\n시작하려면 \"추가\"를 클릭하십시오."},
                {"fr","Aucun mappage de port\r\n\r\nCliquez sur \"Ajouter\" pour commencer."},
                {"it","Nessuna mappatura delle porte.\r\n\r\nFai clic su \"Aggiungi\" per iniziare."},
                {"zh-chs","无端口映射\r\n\r\n单击“添加”开始。"},
                {"pt-br","Sem mapeamentos de portas\r\n\r\nClique em \"Adicionar\" para começar."},
                {"sv","Inga portkartor\r\n\r\nKlicka på \"Lägg till\" för att komma igång."},
                {"es","Sin Asignaciones de Puertos\r\n\r\nHaz clic en \"Agregar\" para comenzar."},
                {"hi","कोई पोर्ट मैपिंग नहीं\r\n\r\nआरंभ करने के लिए \"जोड़ें\" पर क्लिक करें।"},
                {"de","Keine Portzuordnungen\r\n\r\nKlicken Sie auf \"Hinzufügen\", um zu beginnen."}
            }
        },
        {
            "label1",
            new Dictionary<string, string>() {
                {"fr","étiquette1"},
                {"it","etichetta1"},
                {"zh-chs","标签 1"},
                {"pt-br","etiqueta1"},
                {"sv","etikett1"},
                {"es","etiqueta1"},
                {"hi","लेबल1"},
                {"de","Etikett1"}
            }
        },
        {
            "Server",
            new Dictionary<string, string>() {
                {"ko","서버"},
                {"fr","Serveur"},
                {"zh-chs","服务器"},
                {"pt-br","Servidor"},
                {"es","Servidor"},
                {"hi","सर्वर"}
            }
        },
        {
            "Log out",
            new Dictionary<string, string>() {
                {"nl","Uitloggen"},
                {"ko","로그 아웃"},
                {"fr","Se déconnecter"},
                {"it","Disconnettersi"},
                {"zh-chs","登出"},
                {"pt-br","Sair"},
                {"sv","Logga ut"},
                {"es","Cerrar sesión"},
                {"hi","लॉग आउट"},
                {"de","Ausloggen"}
            }
        },
        {
            "Don't ask for {0} days.",
            new Dictionary<string, string>() {
                {"nl","Niet vragen voor {0} dagen."},
                {"ko","{0} 일을 요구하지 마세요."},
                {"fr","Ne demandez pas {0} jours."},
                {"it","Non chiedere per  {0} giorni."},
                {"zh-chs","不要要求 {0} 天。"},
                {"pt-br","Não pergunte por {0} dias."},
                {"sv","Be inte om {0} dagar."},
                {"es","No pedir por {0} días."},
                {"hi","{0} दिनों के लिए मत पूछो।"},
                {"de","Frag nicht nach {0} Tagen."}
            }
        },
        {
            "Forward all keyboard keys",
            new Dictionary<string, string>() {
                {"nl","Alle toetsenbordtoetsen doorsturen"},
                {"ko","모든 키보드 키 전달"},
                {"fr","Transférer toutes les touches du clavier"},
                {"it","Inoltra tutti i tasti della tastiera"},
                {"zh-chs","转发所有键盘键"},
                {"pt-br","Encaminhar todas as teclas do teclado"},
                {"sv","Vidarebefordra alla tangentbordstangenter"},
                {"es","Reenviar todas las teclas del teclado"},
                {"hi","सभी कीबोर्ड कुंजियों को अग्रेषित करें"},
                {"de","Alle Tastaturtasten weiterleiten"}
            }
        },
        {
            "Agent",
            new Dictionary<string, string>() {
                {"hi","एजेंट"},
                {"zh-cht","代理"},
                {"zh-chs","代理"},
                {"fi","Agentti"},
                {"tr","Ajan"},
                {"ja","エージェント"},
                {"es","Agente"},
                {"pt","Agente"},
                {"pt-br","Agente"},
                {"ko","에이전트"},
                {"it","Agente"},
                {"ru","Агент"}
            }
        },
        {
            "Ask Consent",
            new Dictionary<string, string>() {
                {"de","Einwilligung anfordern"},
                {"hi","सहमति से पूछें"},
                {"fr","Demander le consentement"},
                {"zh-cht","詢問同意"},
                {"zh-chs","询问同意"},
                {"fi","Pyydä Hyväksyntää"},
                {"tr","Onay isteyin"},
                {"cs","Požádat o souhlas"},
                {"ja","同意を求める"},
                {"es","Pedir Consentimiento"},
                {"pt","Peça consentimento"},
                {"nl","Vraag toestemming"},
                {"pt-br","Peça consentimento"},
                {"sv","Fråga samtycke"},
                {"ko","연결 요청"},
                {"it","Chiedi il consenso"},
                {"ru","Спросите согласия"}
            }
        },
        {
            "Add &Map...",
            new Dictionary<string, string>() {
                {"nl","&Toewijzing toevoegen..."},
                {"ko","지도 추가 ..."},
                {"fr","Ajouter une &carte..."},
                {"it","Aggiungi &mappa..."},
                {"zh-chs","添加地图 (&M)..."},
                {"pt-br","Adicionar & mapear ..."},
                {"sv","Lägg till & mappa ..."},
                {"es","Agregar & mapa ..."},
                {"hi","नक्शा जोड़ें..."},
                {"de","&Zuordnen hinzufügen..."}
            }
        },
        {
            "Stopped",
            new Dictionary<string, string>() {
                {"nl","Gestopt"},
                {"ko","중지됨"},
                {"fr","Arrêté"},
                {"it","Fermato"},
                {"zh-chs","停止"},
                {"pt-br","Parou"},
                {"sv","Stannade"},
                {"es","Detenido"},
                {"hi","रोका हुआ"},
                {"de","Gestoppt"}
            }
        },
        {
            "Two-factor Authentication",
            new Dictionary<string, string>() {
                {"nl","Twee-factor authenticatie"},
                {"ko","2 단계 인증"},
                {"fr","Authentification à deux facteurs"},
                {"it","Autenticazione a due fattori"},
                {"zh-chs","两因素身份验证"},
                {"pt-br","Autenticação de dois fatores"},
                {"sv","Tvåfaktorsautentisering"},
                {"es","Autenticación de Dos Factores"},
                {"hi","दो तरीकों से प्रमाणीकरण"},
                {"de","Zwei-Faktor-Authentifizierung"}
            }
        },
        {
            "Update",
            new Dictionary<string, string>() {
                {"de","Updates"},
                {"hi","अपडेट करें"},
                {"fr","Mettre à jour"},
                {"zh-cht","更新資料"},
                {"zh-chs","更新资料"},
                {"fi","Päivittää"},
                {"tr","Güncelleme"},
                {"cs","Aktualizace"},
                {"ja","更新"},
                {"es","Actualizar"},
                {"pt","Atualizar"},
                {"nl","Bijwerken"},
                {"pt-br","Atualizar"},
                {"sv","Uppdatering"},
                {"ko","개조하다"},
                {"it","Aggiornamenti"},
                {"ru","Обновить"}
            }
        },
        {
            "Quality",
            new Dictionary<string, string>() {
                {"de","Qualität"},
                {"hi","गुणवत्ता"},
                {"fr","Qualité"},
                {"zh-cht","品質"},
                {"zh-chs","质量"},
                {"fi","Laatu"},
                {"tr","Kalite"},
                {"cs","Kvalita"},
                {"ja","品質"},
                {"es","Calidad"},
                {"pt","Qualidade"},
                {"nl","Kwaliteit"},
                {"pt-br","Qualidade"},
                {"sv","Kvalitet"},
                {"ko","품질"},
                {"it","Qualità"},
                {"ru","Качество"}
            }
        },
        {
            "View Certificate Details...",
            new Dictionary<string, string>() {
                {"nl","Certificaatdetails bekijken..."},
                {"ko","인증서 세부 정보보기 ..."},
                {"fr","Afficher les détails du certificat..."},
                {"it","Visualizza dettagli certificato..."},
                {"zh-chs","查看证书详细信息..."},
                {"pt-br","Exibir detalhes do certificado ..."},
                {"sv","Visa certifikatinformation ..."},
                {"es","Ver Detalles del Certificado ..."},
                {"hi","प्रमाणपत्र विवरण देखें..."},
                {"de","Zertifikatdetails anzeigen..."}
            }
        },
        {
            "Remote Files...",
            new Dictionary<string, string>() {
                {"nl","Externe bestanden..."},
                {"ko","원격 파일 ..."},
                {"fr","Fichiers distants..."},
                {"it","File remoti..."},
                {"zh-chs","远程文件..."},
                {"pt-br","Arquivos remotos ..."},
                {"sv","Fjärrfiler ..."},
                {"es","Archivos Remotos ..."},
                {"hi","दूरस्थ फ़ाइलें..."},
                {"de","Remote-Dateien..."}
            }
        },
        {
            ", Recorded Session",
            new Dictionary<string, string>() {
                {"nl",", opgenomen sessie"},
                {"ko",", 녹화 된 세션"},
                {"fr",", Séance enregistrée"},
                {"it",", Sessione registrata"},
                {"zh-chs",", 录制会话"},
                {"pt-br",", Sessão Gravada"},
                {"sv",", Inspelad session"},
                {"es",", Sesión grabada"},
                {"hi",", रिकॉर्ड किया गया सत्र"},
                {"de",", Aufgezeichnete Sitzung"}
            }
        },
        {
            "Email verification required",
            new Dictionary<string, string>() {
                {"nl","E-mailverificatie vereist"},
                {"ko","이메일 확인 필요"},
                {"fr","Vérification de l'e-mail requise"},
                {"it","Verifica e-mail richiesta"},
                {"zh-chs","需要电子邮件验证"},
                {"pt-br","Verificação de e-mail necessária"},
                {"sv","Verifiering av e-post krävs"},
                {"es","Se requiere verificación de correo electrónico"},
                {"hi","ईमेल सत्यापन आवश्यक"},
                {"de","E-Mail-Verifizierung erforderlich"}
            }
        },
        {
            "Ignore",
            new Dictionary<string, string>() {
                {"nl","Negeren"},
                {"ko","무시"},
                {"fr","Ignorer"},
                {"it","Ignorare"},
                {"zh-chs","忽略"},
                {"pt-br","Ignorar"},
                {"sv","Strunta i"},
                {"es","Ignorar"},
                {"hi","नज़रअंदाज़ करना"},
                {"de","Ignorieren"}
            }
        },
        {
            "Relay Mapping",
            new Dictionary<string, string>() {
                {"nl","Doorstuurtoewijzing"},
                {"ko","릴레이 매핑"},
                {"fr","Cartographie des relais"},
                {"it","Mappatura ritrasmissione"},
                {"zh-chs","中继映射"},
                {"pt-br","Mapeamento de retransmissão"},
                {"sv","Relämappning"},
                {"es","Mapeo de Relés"},
                {"hi","रिले मैपिंग"},
                {"de","Relaiszuordnung"}
            }
        },
        {
            ", {0} connections.",
            new Dictionary<string, string>() {
                {"nl",", {0} verbindingen."},
                {"ko",", {0} 명 연결."},
                {"fr",", {0} connexions."},
                {"it",", {0} connessioni."},
                {"zh-chs",", {0} 个连接。"},
                {"pt-br",", {0} conexões."},
                {"sv",", {0} anslutningar."},
                {"es",", {0} conexiones."},
                {"hi",", {0} कनेक्शन।"},
                {"de",", {0} Verbindungen."}
            }
        },
        {
            "Enter the RDP port of the remote computer, the default RDP port is 3389.",
            new Dictionary<string, string>() {
                {"nl","Voer de RDP poort van de externe computer in, de standaard RDP poort is 3389."},
                {"ko","원격 컴퓨터의 RDP 포트를 입력합니다. 기본 RDP 포트는 3389입니다."},
                {"fr","Entrez le port RDP de l'ordinateur distant, le port RDP par défaut est 3389."},
                {"it","Immettere la porta RDP del computer remoto, la porta RDP predefinita è 3389."},
                {"zh-chs","输入远程计算机的RDP端口，默认RDP端口为3389。"},
                {"pt-br","Insira a porta RDP do computador remoto, a porta RDP padrão é 3389."},
                {"sv","Ange RDP-porten på fjärrdatorn, standard-RDP-porten är 3389."},
                {"es","Ingresa el puerto RDP de la computadora remota, el puerto RDP predeterminado es 3389."},
                {"hi","दूरस्थ कंप्यूटर का RDP पोर्ट दर्ज करें, डिफ़ॉल्ट RDP पोर्ट 3389 है।"},
                {"de","Geben Sie den RDP-Port des Remote-Computers ein, der Standard-RDP-Port ist 3389."}
            }
        },
        {
            "Confirm Delete",
            new Dictionary<string, string>() {
                {"nl","Verwijderen bevestigen"},
                {"ko","삭제 확인"},
                {"fr","Confirmation de la suppression"},
                {"it","Conferma cancellazione"},
                {"zh-chs","确认删除"},
                {"pt-br","Confirmar exclusão"},
                {"sv","Bekräfta radering"},
                {"es","Confirmar Eliminación"},
                {"hi","हटाने की पुष्टि करें"},
                {"de","Löschen bestätigen"}
            }
        },
        {
            "Updating...",
            new Dictionary<string, string>() {
                {"nl","Bijwerken..."},
                {"ko","업데이트 중 ..."},
                {"fr","Mise à jour..."},
                {"it","In aggiornamento..."},
                {"zh-chs","正在更新..."},
                {"pt-br","Atualizando ..."},
                {"sv","Uppdaterar ..."},
                {"es","Actualizando ..."},
                {"hi","अपडेट हो रहा है..."},
                {"de","Aktualisierung..."}
            }
        },
        {
            "&Delete",
            new Dictionary<string, string>() {
                {"nl","&Verwijderen"},
                {"ko","&지우다"},
                {"fr","&Effacer"},
                {"it","&Elimina"},
                {"zh-chs","＆删除"},
                {"pt-br","&Excluir"},
                {"sv","&Radera"},
                {"es","&Borrar"},
                {"hi","&हटाएं"},
                {"de","&Löschen"}
            }
        },
        {
            "&Info...",
            new Dictionary<string, string>() {
                {"ko","정보 ..."},
                {"zh-chs","＆信息..."},
                {"pt-br","& Info ..."},
                {"es","&Info ..."},
                {"hi","जानकारी..."}
            }
        },
        {
            "Invalid username or password",
            new Dictionary<string, string>() {
                {"nl","Ongeldige gebruikersnaam of wachtwoord"},
                {"ko","잘못된 사용자 이름 또는 비밀번호"},
                {"fr","Nom d'utilisateur ou mot de passe invalide"},
                {"it","Username o password non validi"},
                {"zh-chs","无效的用户名或密码"},
                {"pt-br","nome de usuário ou senha inválidos"},
                {"sv","Ogiltigt användarnamn eller lösenord"},
                {"es","Usuario o contraseña invalido"},
                {"hi","अमान्य उपयोगकर्ता नाम या पासवर्ड"},
                {"de","ungültiger Benutzername oder Passwort"}
            }
        },
        {
            "Rename File",
            new Dictionary<string, string>() {
                {"nl","Bestand hernoemen"},
                {"ko","파일명 변경"},
                {"fr","Renommer le fichier"},
                {"it","Rinomina il file"},
                {"zh-chs","重新命名文件"},
                {"pt-br","Renomear arquivo"},
                {"sv","Döp om fil"},
                {"es","Renombrar Archivo"},
                {"hi","फाइल का नाम बदलो"},
                {"de","Datei umbenennen"}
            }
        },
        {
            "Email",
            new Dictionary<string, string>() {
                {"de","E-Mail"},
                {"hi","ईमेल"},
                {"zh-cht","電郵"},
                {"zh-chs","电邮"},
                {"fi","Sähköposti"},
                {"tr","E-posta"},
                {"cs","E-mail"},
                {"ja","Eメール"},
                {"es","Correo electrónico"},
                {"nl","E-mail"},
                {"pt-br","E-mail"},
                {"sv","E-post"},
                {"ko","이메일"},
                {"it","E-mail"}
            }
        },
        {
            "Display connection statistics",
            new Dictionary<string, string>() {
                {"nl","Verbindingsstatistieken weergeven"},
                {"ko","연결 통계 표시"},
                {"fr","Afficher les statistiques de connexion"},
                {"it","Visualizza le statistiche di connessione"},
                {"zh-chs","显示连接统计"},
                {"pt-br","Exibir estatísticas de conexão"},
                {"sv","Visa anslutningsstatistik"},
                {"es","Mostrar estadísticas de conexión"},
                {"hi","कनेक्शन आंकड़े प्रदर्शित करें"},
                {"de","Verbindungsstatistik anzeigen"}
            }
        },
        {
            "ServerName",
            new Dictionary<string, string>() {
                {"nl","Servernaam"},
                {"ko","서버 이름"},
                {"fr","Nom du serveur"},
                {"it","Nome del server"},
                {"zh-chs","服务器名称"},
                {"pt-br","Nome do servidor"},
                {"sv","Server namn"},
                {"es","Nombre del Servidor"},
                {"hi","सर्वर का नाम"},
                {"de","Servername"}
            }
        },
        {
            "0 Bytes",
            new Dictionary<string, string>() {
                {"ko","0 바이트"},
                {"fr","0 octet"},
                {"zh-chs","0 字节"},
                {"sv","0 byte"},
                {"es","0 bytes"},
                {"hi","0 बाइट्स"},
                {"de","0 Byte"}
            }
        },
        {
            "Received invalid network data",
            new Dictionary<string, string>() {
                {"de","Ungültige Netzwerkdaten empfangen"},
                {"hi","अमान्य नेटवर्क डेटा प्राप्त किया"},
                {"fr","Données réseau reçues non valides"},
                {"zh-cht","收到無效的網絡數據"},
                {"zh-chs","收到无效的网络数据"},
                {"fi","Vastaanotettu virheellisiä verkkotietoja"},
                {"tr","Geçersiz ağ verileri alındı"},
                {"cs","Přijata neplatná síťová data"},
                {"ja","無効なネットワークデータを受信しました"},
                {"es","Se recibieron datos de red inválidos"},
                {"pt","Dados de rede inválidos recebidos"},
                {"nl","Ongeldige netwerkgegevens ontvangen"},
                {"pt-br","Dados de rede inválidos recebidos"},
                {"sv","Mottog ogiltig nätverksdata"},
                {"ko","잘못된 네트워크 데이터를 받았습니다."},
                {"it","Dati di rete non validi ricevuti"},
                {"ru","Получены неверные сетевые данные"}
            }
        },
        {
            "Port {0} to port {1}",
            new Dictionary<string, string>() {
                {"nl","Poort {0} naar poort {1}"},
                {"ko","포트 {0}에서 포트 {1}로"},
                {"fr","Port {0} vers port {1}"},
                {"it","Porta {0} a porta {1}"},
                {"zh-chs","端口 {0} 到端口 {1}"},
                {"pt-br","Porta {0} para porta {1}"},
                {"sv","Porta {0} till port {1}"},
                {"es","Puerto {0} al puerto {1}"},
                {"hi","पोर्ट {0} से पोर्ट {1}"},
                {"de","Port {0} zu Port {1}"}
            }
        },
        {
            "Desktop Settings",
            new Dictionary<string, string>() {
                {"nl","Bureaubladinstellingen"},
                {"ko","데스크탑 설정"},
                {"fr","Paramètres du bureau"},
                {"it","Impostazioni del desktop"},
                {"zh-chs","桌面设置"},
                {"pt-br","Configurações da área de trabalho"},
                {"sv","Skrivbordsinställningar"},
                {"es","Configuración de escritorio"},
                {"hi","डेस्कटॉप सेटिंग्स"},
                {"de","Desktop-Einstellungen"}
            }
        },
        {
            "Connecting...",
            new Dictionary<string, string>() {
                {"de","Verbinden..."},
                {"hi","कनेक्ट ..."},
                {"fr","En cours de connexion ..."},
                {"zh-cht","正在連線..."},
                {"zh-chs","正在连线..."},
                {"fi","Yhdistetään..."},
                {"tr","Bağlanıyor..."},
                {"cs","Připojování…"},
                {"ja","接続しています..."},
                {"es","Conectando...."},
                {"pt","Conectando..."},
                {"nl","Verbinden..."},
                {"pt-br","Conectando ..."},
                {"sv","Ansluter..."},
                {"ko","연결 중 ..."},
                {"it","Collegamento ..."},
                {"ru","Подключение..."}
            }
        },
        {
            "Server information",
            new Dictionary<string, string>() {
                {"nl","Server informatie"},
                {"ko","서버 정보"},
                {"fr","Informations sur le serveur"},
                {"it","Informazioni sul server"},
                {"zh-chs","服务器信息"},
                {"pt-br","Informação do servidor"},
                {"sv","Serverinformation"},
                {"es","Información del Servidor"},
                {"hi","सर्वर जानकारी"},
                {"de","Serverinformation"}
            }
        },
        {
            "Push local clipboard to remote device",
            new Dictionary<string, string>() {
                {"nl","Verplaats lokaal klembord naar extern apparaat"},
                {"ko","로컬 클립 보드를 원격 장치로 푸시"},
                {"fr","Transférer le presse-papiers local vers l'appareil distant"},
                {"it","Spingi gli appunti locali sul dispositivo remoto"},
                {"zh-chs","将本地剪贴板推送到远程设备"},
                {"pt-br","Envie a área de transferência local para o dispositivo remoto"},
                {"sv","Tryck lokalt urklipp till fjärrenheten"},
                {"es","Empuje el portapapeles local al dispositivo remoto"},
                {"hi","स्थानीय क्लिपबोर्ड को दूरस्थ डिवाइस पर पुश करें"},
                {"de","Lokale Zwischenablage auf Remote-Gerät übertragen"}
            }
        },
        {
            "Incoming Bytes",
            new Dictionary<string, string>() {
                {"nl","Inkomende Bytes"},
                {"ko","들어오는 바이트"},
                {"fr","Octets entrants"},
                {"it","Byte in entrata"},
                {"zh-chs","传入字节"},
                {"pt-br","Bytes de entrada"},
                {"sv","Inkommande byte"},
                {"es","Bytes entrantes"},
                {"hi","आने वाली बाइट्स"},
                {"de","Eingehende Bytes"}
            }
        },
        {
            "MeshCentral",
            new Dictionary<string, string>() {
                {"zh-chs","网格中心"},
                {"hi","मेशसेंट्रल"}
            }
        },
        {
            "Show on system tray",
            new Dictionary<string, string>() {
                {"nl","Weergeven in systeemvak"},
                {"ko","시스템 트레이에 표시"},
                {"fr","Afficher sur la barre d'état système"},
                {"it","Mostra sulla barra delle applicazioni"},
                {"zh-chs","在系统托盘上显示"},
                {"pt-br","Mostrar na bandeja do sistema"},
                {"sv","Visa i systemfältet"},
                {"es","Mostrar en la bandeja del sistema"},
                {"hi","सिस्टम ट्रे पर दिखाएं"},
                {"de","In der Taskleiste anzeigen"}
            }
        },
        {
            "E&xit",
            new Dictionary<string, string>() {
                {"nl","Sluiten"},
                {"ko","출구"},
                {"fr","Sortir"},
                {"zh-chs","出口"},
                {"pt-br","Saída"},
                {"sv","Utgång"},
                {"es","Salida"},
                {"hi","बाहर जाएं"},
                {"de","Beenden"}
            }
        },
        {
            "Remote - {0}",
            new Dictionary<string, string>() {
                {"nl","Afstandsbediening - {0}"},
                {"ko","원격-{0}"},
                {"fr","À distance - {0}"},
                {"it","Remoto - {0}"},
                {"zh-chs","远程 - {0}"},
                {"pt-br","Remoto - {0}"},
                {"sv","Fjärrkontroll - {0}"},
                {"es","Remoto: {0}"},
                {"hi","रिमोट - {0}"},
                {"de","Fernbedienung - {0}"}
            }
        },
        {
            "Size",
            new Dictionary<string, string>() {
                {"de","Größe"},
                {"hi","आकार"},
                {"fr","Taille"},
                {"zh-cht","尺寸"},
                {"zh-chs","尺寸"},
                {"fi","Koko"},
                {"tr","Boyut"},
                {"cs","Velikost"},
                {"ja","サイズ"},
                {"es","Tamaño"},
                {"pt","Tamanho"},
                {"nl","Grootte"},
                {"pt-br","Tamanho"},
                {"sv","Storlek"},
                {"ko","크기"},
                {"it","Dimensione"},
                {"ru","Размер"}
            }
        },
        {
            "Site",
            new Dictionary<string, string>() {
                {"nl","Lokatie"},
                {"ko","대지"},
                {"fr","Placer"},
                {"it","Sito"},
                {"zh-chs","地点"},
                {"pt-br","Local"},
                {"sv","Webbplats"},
                {"es","Sitio"},
                {"hi","साइट"},
                {"de","Seite? ˅"}
            }
        },
        {
            "Bind local port to all network interfaces",
            new Dictionary<string, string>() {
                {"nl","Bind lokale poort aan alle netwerkinterfaces"},
                {"ko","모든 네트워크 인터페이스에 로컬 포트 ​​바인딩"},
                {"fr","Lier le port local à toutes les interfaces réseau"},
                {"it","Associa la porta locale a tutte le interfacce di rete"},
                {"zh-chs","将本地端口绑定到所有网络接口"},
                {"pt-br","Vincule a porta local a todas as interfaces de rede"},
                {"sv","Binda lokal port till alla nätverksgränssnitt"},
                {"es","Vincular el puerto local a todas las interfaces de red"},
                {"hi","स्थानीय पोर्ट को सभी नेटवर्क इंटरफेस से बाइंड करें"},
                {"de","Binden Sie den lokalen Port an alle Netzwerkschnittstellen"}
            }
        },
        {
            "Failed to start remote desktop session",
            new Dictionary<string, string>() {
                {"nl","Kan extern bureaubladsessie niet starten"},
                {"ko","원격 데스크톱 세션을 시작하지 못했습니다."},
                {"fr","Échec du démarrage de la session de bureau à distance"},
                {"it","Impossibile avviare la sessione desktop remoto"},
                {"zh-chs","无法启动远程桌面会话"},
                {"pt-br","Falha ao iniciar sessão de área de trabalho remota"},
                {"sv","Det gick inte att starta fjärrskrivbordssessionen"},
                {"es","No se pudo iniciar la sesión de escritorio remoto"},
                {"hi","दूरस्थ डेस्कटॉप सत्र प्रारंभ करने में विफल"},
                {"de","Fehler beim Starten der Remote-Desktop-Sitzung"}
            }
        },
        {
            "Connection",
            new Dictionary<string, string>() {
                {"nl","Verbinding"},
                {"ko","연결"},
                {"fr","Lien"},
                {"it","Connessione"},
                {"zh-chs","联系"},
                {"pt-br","Conexão"},
                {"sv","Förbindelse"},
                {"es","Conexión"},
                {"hi","संबंध"},
                {"de","Verbindung"}
            }
        },
        {
            "Click ok to register MeshCentral Router on your system as the handler for the \"mcrouter://\" protocol. This will allow the MeshCentral web site to launch this application when needed.",
            new Dictionary<string, string>() {
                {"nl","Klik op ok om MeshCentral Router op uw systeem te registreren als de handler voor het \"mcrouter://\" protocol. Hierdoor kan de MeshCentral-website deze applicatie starten wanneer dat nodig is."},
                {"ko","확인을 클릭하여 \"mcrouter : //\"프로토콜의 핸들러로 시스템에 MeshCentral 라우터를 등록하십시오. 이렇게하면 필요할 때 MeshCentral 웹 사이트에서이 응용 프로그램을 시작할 수 있습니다."},
                {"fr","Cliquez sur ok pour enregistrer MeshCentral Router sur votre système en tant que gestionnaire du protocole « mcrouter:// ». Cela permettra au site Web MeshCentral de lancer cette application en cas de besoin."},
                {"it","Fare clic su OK per registrare MeshCentral Router sul sistema come gestore per il protocollo \"mcrouter://\". Ciò consentirà al sito Web MeshCentral di avviare questa applicazione quando necessario."},
                {"zh-chs","单击确定在您的系统上注册 MeshCentral Router 作为“mcrouter://”协议的处理程序。这将允许 MeshCentral 网站在需要时启动此应用程序。"},
                {"pt-br","Clique em ok para registrar o MeshCentral Router em seu sistema como o manipulador do protocolo \"mcrouter: //\". Isso permitirá que o site MeshCentral inicie esse aplicativo quando necessário."},
                {"sv","Klicka på ok för att registrera MeshCentral Router på ditt system som hanterare för \"mcrouter: //\" -protokollet. Detta gör att MeshCentral-webbplatsen kan starta den här applikationen vid behov."},
                {"es","Haz clic en Aceptar para registrar MeshCentral Router en tu sistema como el controlador del protocolo \"mcrouter: //\". Esto permitirá que el sitio web de MeshCentral inicie esta aplicación cuando sea necesario."},
                {"hi","MeshCentral राउटर को अपने सिस्टम पर \"mcrouter: //\" प्रोटोकॉल के लिए हैंडलर के रूप में पंजीकृत करने के लिए ओके पर क्लिक करें। यह मेशसेंट्रल वेब साइट को जरूरत पड़ने पर इस एप्लिकेशन को लॉन्च करने की अनुमति देगा।"},
                {"de","Klicken Sie auf OK, um MeshCentral Router auf Ihrem System als Handler für das Protokoll \"mcrouter://\" zu registrieren. Dadurch kann die MeshCentral-Website diese Anwendung bei Bedarf starten."}
            }
        },
        {
            "HTTP",
            new Dictionary<string, string>() {
                {"hi","एचटीटीपी"}
            }
        },
        {
            "Local Port",
            new Dictionary<string, string>() {
                {"nl","Lokale poort"},
                {"ko","로컬 포트"},
                {"fr","Port local"},
                {"it","Porta locale"},
                {"zh-chs","本地端口"},
                {"pt-br","Porto Local"},
                {"sv","Lokal hamn"},
                {"es","Puerto Local"},
                {"hi","स्थानीय बंदरगाह"},
                {"de","Lokaler Hafen"}
            }
        },
        {
            "Open Web Site",
            new Dictionary<string, string>() {
                {"nl","Website openen"},
                {"ko","웹 사이트 열기"},
                {"fr","Ouvrir le site Web"},
                {"it","Apri sito web"},
                {"zh-chs","打开网站"},
                {"pt-br","Abra o site"},
                {"sv","Öppna webbplatsen"},
                {"es","Abrir Sitio Web"},
                {"hi","वेब साइट खोलें"},
                {"de","Website öffnen"}
            }
        },
        {
            "Enter the second factor authentication token.",
            new Dictionary<string, string>() {
                {"nl","Voer de tweede factor authenticatie token in."},
                {"ko","두 번째 요소 인증 토큰을 입력하십시오."},
                {"fr","Saisissez le jeton d'authentification du deuxième facteur."},
                {"it","Immettere il token di autenticazione del secondo fattore."},
                {"zh-chs","输入第二个因素身份验证令牌。"},
                {"pt-br","Insira o token de autenticação de segundo fator."},
                {"sv","Ange den andra faktor-autentiseringstoken."},
                {"es","Ingresa el token del segundo factor de autenticación."},
                {"hi","दूसरा कारक प्रमाणीकरण टोकन दर्ज करें।"},
                {"de","Geben Sie das zweite Faktor-Authentifizierungstoken ein."}
            }
        },
        {
            "Remote Desktop...",
            new Dictionary<string, string>() {
                {"nl","Extern bureaublad..."},
                {"ko","원격 데스크탑..."},
                {"fr","Bureau à distance..."},
                {"it","Desktop remoto..."},
                {"zh-chs","远程桌面..."},
                {"pt-br","Área de trabalho remota..."},
                {"sv","Fjärrskrivbord..."},
                {"es","Escritorio Remoto..."},
                {"hi","रिमोट डेस्कटॉप..."},
                {"de","Remotedesktop..."}
            }
        },
        {
            "Remote Desktop",
            new Dictionary<string, string>() {
                {"de","Remotedesktop"},
                {"hi","रिमोट डेस्कटॉप"},
                {"fr","Bureau distant"},
                {"zh-cht","遠程桌面"},
                {"zh-chs","远程桌面"},
                {"fi","Etätyöpöytä"},
                {"tr","Uzak Masaüstü"},
                {"cs","Vzdálená plocha"},
                {"ja","リモートデスクトップ"},
                {"es","Escritorio Remoto"},
                {"pt","Área de trabalho remota"},
                {"nl","Extern bureaublad"},
                {"pt-br","Área de trabalho remota"},
                {"sv","Fjärrskrivbord"},
                {"ko","원격 데스크탑"},
                {"it","Desktop remoto"},
                {"ru","Удаленного рабочего стола"}
            }
        },
        {
            "&Open...",
            new Dictionary<string, string>() {
                {"ko","&열다..."},
                {"fr","&Ouvert..."},
                {"it","&Apri..."},
                {"zh-chs","＆打开..."},
                {"pt-br","&Aberto..."},
                {"sv","&Öppna..."},
                {"es","&Abierto..."},
                {"hi","&खुला हुआ..."},
                {"de","&Öffnen..."}
            }
        },
        {
            "Tunnelling Data",
            new Dictionary<string, string>() {
                {"nl","Gegevens tunnelen"},
                {"ko","터널링 데이터"},
                {"fr","Données de tunneling"},
                {"it","Dati di tunneling"},
                {"zh-chs","隧道数据"},
                {"pt-br","Dados de tunelamento"},
                {"sv","Tunneldata"},
                {"es","Datos de Tunelización"},
                {"hi","टनलिंग डेटा"},
                {"de","Tunneling-Daten"}
            }
        },
        {
            "Device Settings",
            new Dictionary<string, string>() {
                {"nl","Apparaat instellingen"},
                {"ko","기기 설정"},
                {"fr","Réglages de l'appareil"},
                {"it","Impostazioni del dispositivo"},
                {"zh-chs","设备设置"},
                {"pt-br","Configurações do dispositivo"},
                {"sv","Enhetsinställningar"},
                {"es","Configuración de dispositivo"},
                {"hi","उपकरण सेटिंग्स"},
                {"de","Geräteeinstellungen"}
            }
        },
        {
            "MeshCentral Router Installation",
            new Dictionary<string, string>() {
                {"nl","MeshCentral Router Installatie"},
                {"ko","MeshCentral 라우터 설치"},
                {"fr","Installation du routeur MeshCentral"},
                {"it","Installazione del router MeshCentral"},
                {"zh-chs","MeshCentral 路由器安装"},
                {"pt-br","Instalação do roteador MeshCentral"},
                {"es","Instalación del enrutador MeshCentral"},
                {"hi","मेशसेंट्रल राउटर इंस्टालेशन"},
                {"de","Installation des MeshCentral-Routers"}
            }
        },
        {
            "Remote Files",
            new Dictionary<string, string>() {
                {"nl","Externe bestanden"},
                {"ko","원격 파일"},
                {"fr","Fichiers distants"},
                {"it","File remoti"},
                {"zh-chs","远程文件"},
                {"pt-br","Arquivos Remotos"},
                {"sv","Fjärrfiler"},
                {"es","Archivos Remotos"},
                {"hi","दूरस्थ फ़ाइलें"},
                {"de","Remote-Dateien"}
            }
        },
        {
            "Incoming Compression",
            new Dictionary<string, string>() {
                {"nl","Inkomende compressie"},
                {"ko","들어오는 압축"},
                {"fr","Compression entrante"},
                {"it","Compressione in entrata"},
                {"zh-chs","传入压缩"},
                {"pt-br","Compressão de entrada"},
                {"sv","Inkommande kompression"},
                {"es","Compresión entrante"},
                {"hi","आने वाली संपीड़न"},
                {"de","Eingehende Kompression"}
            }
        },
        {
            "0%",
            new Dictionary<string, string>() {
                {"ko","0 %"}
            }
        },
        {
            "Remote Device",
            new Dictionary<string, string>() {
                {"nl","Extern apparaat"},
                {"ko","원격 장치"},
                {"fr","Périphérique distant"},
                {"it","Dispositivo remoto"},
                {"zh-chs","遥控装置"},
                {"pt-br","Dispositivo Remoto"},
                {"sv","Fjärrenhet"},
                {"es","Dispositivo Remoto"},
                {"hi","रिमोट डिवाइस"},
                {"de","Remote-Gerät"}
            }
        },
        {
            "Unable to connect",
            new Dictionary<string, string>() {
                {"nl","Niet in staat te verbinden"},
                {"ko","연결할 수 없습니다"},
                {"fr","Impossible de se connecter"},
                {"it","Impossibile connetersi"},
                {"zh-chs","无法连接"},
                {"pt-br","Incapaz de conectar "},
                {"sv","Kan inte ansluta"},
                {"es","No se puede conectar"},
                {"hi","कनेक्ट करने में असमर्थ"},
                {"de","Verbindung konnte nicht hergestellt werden"}
            }
        },
        {
            "MQTT",
            new Dictionary<string, string>() {
                {"it","MQTT "}
            }
        },
        {
            "Ask Consent + Bar",
            new Dictionary<string, string>() {
                {"de","Einwilligung anfordern + DS-Leiste"},
                {"hi","सहमति + बार पूछें"},
                {"fr","Demander le consentement + le bar"},
                {"zh-cht","詢問同意+工具欄"},
                {"zh-chs","询问同意+工具栏"},
                {"fi","Pyydä Hyväksyntää + Ilmoitus"},
                {"tr","Onay Sor + Bar"},
                {"cs","Zeptejte se souhlasu + bar"},
                {"ja","同意を求める+バー"},
                {"es","Pedir Consentimiento + Barra"},
                {"pt","Peça Consentimento + Bar"},
                {"nl","Vraag toestemming + informatiebalk"},
                {"pt-br","Peça Consentimento + Bar"},
                {"sv","Fråga samtycke + bar"},
                {"ko","연결 요청 + Bar"},
                {"it","Chiedi Consenso + Bar"},
                {"ru","Спросите согласия + бар"}
            }
        },
        {
            "{0} Bytes",
            new Dictionary<string, string>() {
                {"ko","{0} 바이트"},
                {"fr","{0} octets"},
                {"zh-chs","{0} 字节"},
                {"pt-br","{0} bytes"},
                {"sv","{0} Byte"},
                {"es","{0} bytes"},
                {"hi","{0} बाइट्स"},
                {"de","{0} Byte"}
            }
        },
        {
            "Show &Group Names",
            new Dictionary<string, string>() {
                {"nl","Toon &groepsnamen"},
                {"ko","그룹 이름 표시"},
                {"fr","Afficher les noms de &groupes"},
                {"it","Mostra i nomi dei gruppi"},
                {"zh-chs","显示组名称(&G)"},
                {"pt-br","Mostrar nomes de grupos"},
                {"sv","Visa & gruppnamn"},
                {"es","Mostrar y nombres de grupos"},
                {"hi","समूह के नाम दिखाएं"},
                {"de","&Gruppennamen anzeigen"}
            }
        },
        {
            "Username",
            new Dictionary<string, string>() {
                {"de","Benutzername"},
                {"hi","उपयोगकर्ता नाम"},
                {"fr","Nom d'utilisateur"},
                {"zh-cht","用戶名"},
                {"zh-chs","用户名"},
                {"fi","Käyttäjätunnus"},
                {"tr","Kullanıcı adı"},
                {"cs","Uživatelské jméno"},
                {"ja","ユーザー名"},
                {"es","Nombre de Usuario"},
                {"pt","Nome de usuário"},
                {"nl","Gebruikersnaam"},
                {"pt-br","Nome do usuário"},
                {"sv","Användarnamn"},
                {"ko","사용자 이름"},
                {"it","Nome utente"},
                {"ru","Имя пользователя"}
            }
        },
        {
            "Disconnected",
            new Dictionary<string, string>() {
                {"de","Getrennt"},
                {"hi","डिस्कनेक्ट किया गया"},
                {"fr","Débranché"},
                {"zh-cht","已斷線"},
                {"zh-chs","已断线"},
                {"fi","Yhteys katkaistu"},
                {"tr","Bağlantı kesildi"},
                {"cs","Odpojeno"},
                {"ja","切断されました"},
                {"es","Desconectado"},
                {"pt","Desconectado"},
                {"nl","Verbroken"},
                {"pt-br","Desconectado"},
                {"sv","Frånkopplad"},
                {"ko","연결 해제"},
                {"it","Disconnesso"},
                {"ru","Отключен"}
            }
        },
        {
            "No Devices",
            new Dictionary<string, string>() {
                {"nl","Geen apparaten"},
                {"ko","장치 없음"},
                {"fr","Aucun appareil"},
                {"it","Nessun Dispositivo"},
                {"zh-chs","没有设备"},
                {"pt-br","Sem dispositivos"},
                {"sv","Inga enheter"},
                {"es","Sin Dispositivos"},
                {"hi","कोई उपकरण नहीं"},
                {"de","Keine Geräte"}
            }
        },
        {
            "State",
            new Dictionary<string, string>() {
                {"de","Zustand"},
                {"hi","राज्य"},
                {"fr","Etat"},
                {"zh-cht","狀態"},
                {"zh-chs","状况"},
                {"fi","Tila"},
                {"tr","Durum"},
                {"cs","Stav"},
                {"ja","状態"},
                {"es","Estado"},
                {"pt","Estado"},
                {"nl","Status"},
                {"pt-br","Estado"},
                {"sv","stat"},
                {"ko","상태"},
                {"it","Stato"},
                {"ru","Состояние"}
            }
        },
        {
            "Email sent",
            new Dictionary<string, string>() {
                {"nl","E-mail verzonden"},
                {"ko","이메일을 보냈습니다."},
                {"fr","Email envoyé"},
                {"it","Email inviata"},
                {"zh-chs","邮件已发送"},
                {"pt-br","Email enviado"},
                {"sv","Email skickat"},
                {"es","Correo electrónico enviado"},
                {"hi","ईमेल भेजा"},
                {"de","E-Mail gesendet"}
            }
        },
        {
            "Add Map...",
            new Dictionary<string, string>() {
                {"nl","Kaart toevoegen..."},
                {"ko","지도 추가 ..."},
                {"fr","Ajouter une carte..."},
                {"it","Aggiungi mappa..."},
                {"zh-chs","添加地图..."},
                {"pt-br","Adicionar mapa ..."},
                {"sv","Lägg till karta ..."},
                {"es","Agregar mapa ..."},
                {"hi","नक्शा जोड़ें..."},
                {"de","Karte hinzufügen..."}
            }
        },
        {
            "SMS sent",
            new Dictionary<string, string>() {
                {"nl","SMS verzonden"},
                {"ko","SMS 전송"},
                {"fr","SMS envoyé"},
                {"it","SMS inviato"},
                {"zh-chs","短信发送"},
                {"pt-br","SMS enviado"},
                {"sv","SMS skickat"},
                {"es","SMS enviado"},
                {"hi","एसएमएस भेजा गया"},
                {"de","SMS gesendet"}
            }
        },
        {
            "Ctrl-Alt-Del",
            new Dictionary<string, string>() {
                {"de","Strg-Alt-Entf"}
            }
        },
        {
            "&Open Mappings...",
            new Dictionary<string, string>() {
                {"nl","&toewijzingen openen..."},
                {"ko","매핑 열기 ..."},
                {"fr","&Ouvrir les mappages..."},
                {"it","&Apri mappature..."},
                {"zh-chs","打开映射 (&O)..."},
                {"pt-br","& Abrir mapeamentos ..."},
                {"sv","& Öppna kartläggningar ..."},
                {"es","&Abrir mapeos ..."},
                {"hi","&ओपन मैपिंग..."},
                {"de","&Zuordnungen öffnen..."}
            }
        },
        {
            "PuTTY SSH client",
            new Dictionary<string, string>() {
                {"ko","PuTTY SSH 클라이언트"},
                {"fr","Client SSH PuTTY"},
                {"zh-chs","PuTTY SSH 客户端"},
                {"pt-br","Cliente PuTTY SSH"},
                {"sv","PuTTY SSH-klient"},
                {"es","Cliente PuTTY SSH"},
                {"hi","पुटी एसएसएच क्लाइंट"},
                {"de","PuTTY SSH-Client"}
            }
        },
        {
            "Invalid download.",
            new Dictionary<string, string>() {
                {"nl","Ongeldige download."},
                {"ko","다운로드가 잘못되었습니다."},
                {"fr","Téléchargement non valide."},
                {"it","Download non valido."},
                {"zh-chs","下载无效。"},
                {"pt-br","Download inválido."},
                {"sv","Ogiltig nedladdning."},
                {"es","Descarga no válida."},
                {"hi","अमान्य डाउनलोड।"},
                {"de","Ungültiger Download."}
            }
        },
        {
            "File Operation",
            new Dictionary<string, string>() {
                {"de","Dateivorgang"},
                {"hi","फ़ाइल ऑपरेशन"},
                {"fr","Opération sur les fichiers"},
                {"zh-cht","檔案操作"},
                {"zh-chs","档案操作"},
                {"fi","Tiedoston käyttö"},
                {"tr","Dosya İşlemi"},
                {"cs","Provoz souboru"},
                {"ja","ファイル操作"},
                {"es","Operación de Archivo"},
                {"pt","Operação de arquivo"},
                {"nl","Bestandsbewerking"},
                {"pt-br","Operação de arquivo"},
                {"sv","Filhantering"},
                {"ko","파일 작업"},
                {"it","Operazione sui file"},
                {"ru","Работа с файлами"}
            }
        },
        {
            "Cancel",
            new Dictionary<string, string>() {
                {"de","Abbrechen"},
                {"hi","रद्द करना"},
                {"fr","Annuler"},
                {"zh-cht","取消"},
                {"zh-chs","取消"},
                {"fi","Peruuta"},
                {"tr","İptal etmek"},
                {"cs","Storno"},
                {"ja","キャンセル"},
                {"es","Cancelar"},
                {"pt","Cancelar"},
                {"nl","Annuleren"},
                {"pt-br","Cancelar"},
                {"sv","Avbryt"},
                {"ko","취소"},
                {"it","Annulla"},
                {"ru","Отмена"}
            }
        },
        {
            "Connected",
            new Dictionary<string, string>() {
                {"de","Verbunden"},
                {"hi","जुड़े हुए"},
                {"fr","Connecté"},
                {"zh-cht","已連接"},
                {"zh-chs","已连接"},
                {"fi","Yhdistetty"},
                {"tr","Bağlandı"},
                {"cs","Připojeno"},
                {"ja","接続済み"},
                {"es","Conectado"},
                {"pt","Conectado"},
                {"nl","Verbonden"},
                {"pt-br","Conectado"},
                {"sv","Ansluten"},
                {"ko","연결됨"},
                {"it","Collegato"},
                {"ru","Подключено"}
            }
        },
        {
            "Display {0}",
            new Dictionary<string, string>() {
                {"de","Anzeige {0}"},
                {"hi","प्रदर्शन {0}"},
                {"fr","Affichage {0}"},
                {"es","Mostrar {0}"},
                {"zh-chs","显示{0}"},
                {"nl","Scherm {0}"},
                {"pt-br","Exibir {0}"},
                {"ru","Экран {0}"},
                {"sv","Visa {0}"},
                {"ko","{0} 디스플레이"}
            }
        },
        {
            "Application Link",
            new Dictionary<string, string>() {
                {"nl","Applicatielink"},
                {"ko","응용 프로그램 링크"},
                {"fr","Lien d'application"},
                {"it","Collegamento all'applicazione"},
                {"zh-chs","申请链接"},
                {"pt-br","Link do aplicativo"},
                {"sv","Applikationslänk"},
                {"es","Enlace de aplicación"},
                {"hi","आवेदन लिंक"},
                {"de","Bewerbungslink"}
            }
        },
        {
            "SMS",
            new Dictionary<string, string>() {
                {"hi","एसएमएस"},
                {"zh-cht","短信"},
                {"zh-chs","短信"},
                {"fi","Tekstiviesti"},
                {"ru","смс"}
            }
        },
        {
            "Routing Status",
            new Dictionary<string, string>() {
                {"nl","Routeringsstatus"},
                {"ko","라우팅 상태"},
                {"fr","État du routage"},
                {"it","Stato del percorso"},
                {"zh-chs","路由状态"},
                {"pt-br","Status de roteamento"},
                {"sv","Ruttstatus"},
                {"es","Estado de Enrutamiento"},
                {"hi","रूटिंग स्थिति"},
                {"de","Routing-Status"}
            }
        },
        {
            "Add Relay Map...",
            new Dictionary<string, string>() {
                {"nl","Toevoegen relay kaart..."},
                {"ko","릴레이 맵 추가 ..."},
                {"fr","Ajouter une carte de relais..."},
                {"it","Aggiungi mappa di rilancio..."},
                {"zh-chs","添加中继地图..."},
                {"pt-br","Adicionar mapa de retransmissão ..."},
                {"sv","Lägg till reläkarta ..."},
                {"es","Agregar mapa de retransmisiones ..."},
                {"hi","रिले मैप जोड़ें..."},
                {"de","Relaiskarte hinzufügen..."}
            }
        },
        {
            "Add &Relay Map...",
            new Dictionary<string, string>() {
                {"nl","&Relay toewijzing toevoegen..."},
                {"ko","릴레이 맵 추가 ..."},
                {"fr","Ajouter une carte de &relais..."},
                {"it","Aggiungi &mappa di inoltro..."},
                {"zh-chs","添加中继地图 (&R)..."},
                {"pt-br","Adicionar & retransmitir mapa ..."},
                {"sv","Lägg till & vidarebefordra karta ..."},
                {"es","Agregar y retransmitir mapa ..."},
                {"hi","मानचित्र &रिले जोड़ें..."},
                {"de","&Relay-Karte hinzufügen..."}
            }
        },
        {
            "Starting...",
            new Dictionary<string, string>() {
                {"nl","Starten..."},
                {"ko","시작하는 중 ..."},
                {"fr","Départ..."},
                {"it","In Avvio"},
                {"zh-chs","开始..."},
                {"pt-br","Iniciando..."},
                {"sv","Startande..."},
                {"es","A partir de..."},
                {"hi","शुरुआत..."},
                {"de","Beginnend..."}
            }
        },
        {
            "Remote IP",
            new Dictionary<string, string>() {
                {"nl","Extern IP"},
                {"ko","원격 IP"},
                {"fr","IP distante"},
                {"it","IP remoto"},
                {"zh-chs","远程IP"},
                {"pt-br","IP Remoto"},
                {"sv","Fjärr-IP"},
                {"es","IP Remota"},
                {"hi","दूरदराज़ के आई. पी"},
                {"de","Remote-IP"}
            }
        },
        {
            "Relay Device",
            new Dictionary<string, string>() {
                {"nl","Doorstuur apparaat"},
                {"ko","릴레이 장치"},
                {"fr","Dispositif de relais"},
                {"it","Dispositivo ripetitore"},
                {"zh-chs","中继装置"},
                {"pt-br","Dispositivo de Relé"},
                {"sv","Reläenhet"},
                {"es","Dispositivo de Retransmisión"},
                {"hi","रिले डिवाइस"},
                {"de","Relaisgerät"}
            }
        },
        {
            "&Rename",
            new Dictionary<string, string>() {
                {"nl","&Hernoemen"},
                {"ko","이름 바꾸기"},
                {"fr","&Renommer"},
                {"it","&Rinominare"},
                {"zh-chs","＆改名"},
                {"pt-br","& Renomear"},
                {"sv","&Döp om"},
                {"es","&Renombrar"},
                {"hi","&नाम बदलें"},
                {"de","&Umbenennen"}
            }
        },
        {
            "Port {0} to {1}:{2}",
            new Dictionary<string, string>() {
                {"nl","Poort {0} naar {1}:{2}"},
                {"ko","{0}에서 {1}로 포트 : {2}"},
                {"fr","Port {0} vers {1} :{2}"},
                {"it","Porta {0} a {1}:{2}"},
                {"zh-chs","端口 {0} 到 {1}：{2}"},
                {"pt-br","Porta {0} para {1}: {2}"},
                {"sv","Port {0} till {1}: {2}"},
                {"es","Puerto {0} a {1}: {2}"},
                {"hi","पोर्ट {0} से {1}:{2}"},
                {"de","Port {0} nach {1}:{2}"}
            }
        },
        {
            "Create Folder",
            new Dictionary<string, string>() {
                {"nl","Map aanmaken"},
                {"ko","폴더 생성"},
                {"fr","Créer le dossier"},
                {"it","Creare una cartella"},
                {"zh-chs","创建文件夹"},
                {"pt-br","Criar pasta"},
                {"sv","Skapa mapp"},
                {"es","Crear Carpeta"},
                {"hi","फोल्डर बनाएं"},
                {"de","Ordner erstellen"}
            }
        },
        {
            "Fast",
            new Dictionary<string, string>() {
                {"de","Schnell"},
                {"hi","तेज"},
                {"fr","Vite"},
                {"zh-cht","快速"},
                {"zh-chs","快速"},
                {"fi","Nopea"},
                {"tr","Hızlı"},
                {"cs","Rychle"},
                {"ja","速い"},
                {"es","Rápido"},
                {"pt","Rápido"},
                {"nl","Snel"},
                {"pt-br","Rápido"},
                {"sv","Snabb"},
                {"ko","빠른"},
                {"it","Veloce"},
                {"ru","Быстро"}
            }
        },
        {
            "Set RDP port...",
            new Dictionary<string, string>() {
                {"nl","RDP poort instellen..."},
                {"ko","RDP 포트 설정 ..."},
                {"fr","Définir le port RDP..."},
                {"it","Imposta porta RDP..."},
                {"zh-chs","设置 RDP 端口..."},
                {"pt-br","Definir porta RDP ..."},
                {"sv","Ställ in RDP-port ..."},
                {"es","Establecer puerto RDP ..."},
                {"hi","आरडीपी पोर्ट सेट करें..."},
                {"de","RDP-Port einstellen..."}
            }
        },
        {
            "Application Launch",
            new Dictionary<string, string>() {
                {"nl","Toepassing starten"},
                {"ko","응용 프로그램 시작"},
                {"fr","Lancement de l'application"},
                {"it","Lancio dell'applicazione"},
                {"zh-chs","应用启动"},
                {"pt-br","Lançamento do aplicativo"},
                {"sv","Applikationsstart"},
                {"es","Lanzamiento de la aplicación"},
                {"hi","एप्लिकेशन लॉन्च"},
                {"de","Anwendungsstart"}
            }
        },
        {
            "No tools allowed",
            new Dictionary<string, string>() {
                {"nl","Geen gereedschap toegestaan"},
                {"fr","Aucun utilitaire autorisé"},
                {"pt-br","Nenhuma ferramenta permitida"},
                {"sv","Inga verktyg tillåtna"},
                {"es","No se permiten herramientas"}
            }
        },
        {
            "File Transfer",
            new Dictionary<string, string>() {
                {"de","Datei Übertragung"},
                {"hi","फ़ाइल स्थानांतरण"},
                {"fr","Transfert de fichiers"},
                {"es","Transferencia de Archivos"},
                {"zh-chs","文件传输"},
                {"nl","Bestandsoverdracht"},
                {"pt-br","Transferência de arquivo"},
                {"ru","Передача файлов"},
                {"sv","Filöverföring"},
                {"ko","파일 전송"},
                {"it","Trasferimento di file"}
            }
        },
        {
            "Transfer Progress",
            new Dictionary<string, string>() {
                {"nl","Voortgang overdracht"},
                {"ko","전송 진행"},
                {"fr","Progression du transfert"},
                {"it","Avanzamento del trasferimento"},
                {"zh-chs","转学进度"},
                {"pt-br","Progresso da Transferência"},
                {"sv","Överför framsteg"},
                {"es","Progreso de la Transferencia"},
                {"hi","स्थानांतरण प्रगति"},
                {"de","Übertragungsfortschritt"}
            }
        },
        {
            "WinSCP client",
            new Dictionary<string, string>() {
                {"ko","WinSCP 클라이언트"},
                {"fr","Client WinSCP"},
                {"it","Client WinSCP"},
                {"zh-chs","WinSCP客户端 "},
                {"pt-br","Cliente WinSCP"},
                {"sv","WinSCP-klient"},
                {"es","Cliente WinSCP"},
                {"hi","विनएससीपी क्लाइंट"},
                {"de","WinSCP-Client"}
            }
        },
        {
            "Back",
            new Dictionary<string, string>() {
                {"de","Zurück"},
                {"hi","वापस"},
                {"fr","Retour"},
                {"zh-cht","返回"},
                {"zh-chs","返回"},
                {"fi","Takaisin"},
                {"tr","Geri"},
                {"cs","Zpět"},
                {"ja","バック"},
                {"es","Atrás"},
                {"pt","Voltar"},
                {"nl","Terug"},
                {"pt-br","Voltar"},
                {"sv","Tillbaka"},
                {"ko","뒤로"},
                {"it","Indietro"},
                {"ru","Назад"}
            }
        },
        {
            "Show &Offline Devices",
            new Dictionary<string, string>() {
                {"nl","Toon &Offline apparaten"},
                {"ko","오프라인 장치 표시 (& O)"},
                {"fr","Afficher les appareils &hors ligne"},
                {"it","Mostra dispositivi offline"},
                {"zh-chs","显示离线设备 (&A)"},
                {"pt-br","Mostrar dispositivos off-line"},
                {"sv","Visa & Offline-enheter"},
                {"es","Mostrar y dispositivos sin conexión"},
                {"hi","दिखाएँ &ऑफ़लाइन उपकरण"},
                {"de","&Offline-Geräte anzeigen"}
            }
        },
        {
            "Application",
            new Dictionary<string, string>() {
                {"nl","Toepassing"},
                {"ko","신청"},
                {"it","Applicazione"},
                {"zh-chs","应用"},
                {"pt-br","Aplicativo"},
                {"sv","Ansökan"},
                {"es","Solicitud"},
                {"hi","आवेदन"},
                {"de","Anwendung"}
            }
        },
        {
            "Rename",
            new Dictionary<string, string>() {
                {"de","Umbenennen"},
                {"hi","नाम बदलें"},
                {"fr","Renommer"},
                {"zh-cht","改名"},
                {"zh-chs","改名"},
                {"fi","Nimeä uudelleen"},
                {"tr","Adını değiştirmek"},
                {"cs","Přejmenovat"},
                {"ja","リネーム"},
                {"es","Renombrar"},
                {"pt","Renomear"},
                {"nl","Hernoemen"},
                {"pt-br","Renomear"},
                {"sv","Döp om"},
                {"ko","이름 바꾸기"},
                {"it","Rinominare"},
                {"ru","Переименовать"}
            }
        },
        {
            "Remove",
            new Dictionary<string, string>() {
                {"de","Entfernen"},
                {"hi","हटाना"},
                {"fr","Retirer"},
                {"zh-cht","删除"},
                {"zh-chs","删除"},
                {"fi","Poista"},
                {"tr","Kaldırmak"},
                {"cs","Odstranit"},
                {"ja","削除する"},
                {"es","Remover"},
                {"pt","Remover"},
                {"nl","Verwijderen"},
                {"pt-br","Remover"},
                {"sv","Avlägsna"},
                {"ko","제거"},
                {"it","Rimuovere"},
                {"ru","Удалить"}
            }
        },
        {
            "Routing Stats",
            new Dictionary<string, string>() {
                {"nl","Routeringsstatistieken"},
                {"ko","라우팅 통계"},
                {"fr","Statistiques de routage"},
                {"it","Statistiche di instradamento"},
                {"zh-chs","路由统计"},
                {"pt-br","Estatísticas de roteamento"},
                {"es","Estadísticas de Enrutamiento"},
                {"hi","रूटिंग आँकड़े"},
                {"de","Routing-Statistiken"}
            }
        },
        {
            "No Search Results",
            new Dictionary<string, string>() {
                {"nl","geen resultaten gevonden"},
                {"ko","검색 결과 없음"},
                {"fr","aucun résultat trouvé"},
                {"it","nessun risultato trovato"},
                {"zh-chs","没有搜索结果"},
                {"pt-br","Sem resultados de pesquisa"},
                {"sv","inga sökresultat"},
                {"es","Sin Resultados de Búsqueda"},
                {"hi","खोजने पर कोई परिणाम नहीं मिला"},
                {"de","keine Suchergebnisse"}
            }
        },
        {
            "Close",
            new Dictionary<string, string>() {
                {"de","Schließen"},
                {"hi","बंद करे"},
                {"fr","Fermer"},
                {"zh-cht","關"},
                {"zh-chs","关"},
                {"fi","Sulje"},
                {"tr","Kapat"},
                {"cs","Zavřít"},
                {"ja","閉じる"},
                {"es","Cerrar"},
                {"pt","Fechar"},
                {"nl","Sluiten"},
                {"pt-br","Perto"},
                {"sv","Stäng"},
                {"ko","닫기"},
                {"it","Chiudere"},
                {"ru","Закрыть"}
            }
        },
        {
            "RDP",
            new Dictionary<string, string>() {
                {"hi","आरडीपी"}
            }
        },
        {
            "OpenSSH",
            new Dictionary<string, string>() {
                {"zh-chs","开放式SSH"},
                {"hi","अधिभारित"}
            }
        },
        {
            "Search",
            new Dictionary<string, string>() {
                {"de","Suche"},
                {"hi","खोज"},
                {"fr","Chercher"},
                {"zh-cht","搜尋"},
                {"zh-chs","搜寻"},
                {"fi","Etsi"},
                {"tr","Arama"},
                {"cs","Hledat"},
                {"ja","サーチ"},
                {"es","Buscar"},
                {"pt","Procurar"},
                {"nl","Zoeken"},
                {"pt-br","Procurar"},
                {"sv","Sök"},
                {"ko","검색"},
                {"it","Ricerca"},
                {"ru","Поиск"}
            }
        },
        {
            "statusStrip1",
            new Dictionary<string, string>() {
                {"it","statoStrip1"},
                {"zh-chs","状态条1"},
                {"hi","स्थिति पट्टी1"}
            }
        },
        {
            "SSH Username",
            new Dictionary<string, string>() {
                {"nl","SSH gebruikersnaam"},
                {"ko","SSH 사용자 이름"},
                {"fr","Nom d'utilisateur SSH"},
                {"it","Nome utente SSH"},
                {"zh-chs","SSH 用户名"},
                {"pt-br","Nome de usuário SSH"},
                {"sv","SSH-användarnamn"},
                {"es","Nombre de usuario SSH"},
                {"hi","एसएसएच उपयोगकर्ता नाम"},
                {"de","SSH-Benutzername"}
            }
        },
        {
            "Medium",
            new Dictionary<string, string>() {
                {"de","Mittel"},
                {"hi","मध्यम"},
                {"fr","Moyen"},
                {"zh-cht","中"},
                {"zh-chs","中"},
                {"fi","Keskikokoinen"},
                {"tr","Orta"},
                {"cs","Středně"},
                {"ja","中"},
                {"es","Medio"},
                {"pt","Médio"},
                {"nl","Gemiddeld"},
                {"pt-br","Médio"},
                {"ko","중간"},
                {"it","medio"},
                {"ru","Средний"}
            }
        },
        {
            "OK",
            new Dictionary<string, string>() {
                {"hi","ठीक"},
                {"fr","ОК"},
                {"tr","tamam"},
                {"pt","Ok"},
                {"ko","확인"},
                {"ru","ОК"}
            }
        },
        {
            "Local - {0}",
            new Dictionary<string, string>() {
                {"nl","Lokaal - {0}"},
                {"ko","지역-{0}"},
                {"fr","Locale - {0}"},
                {"it","Locale - {0}"},
                {"zh-chs","本地 - {0}"},
                {"sv","Lokalt - {0}"},
                {"es","Local: {0}"},
                {"hi","स्थानीय - {0}"},
                {"de","Lokal - {0}"}
            }
        },
        {
            "Send token to registered email address?",
            new Dictionary<string, string>() {
                {"de","Token an registrierte E-Mail-Adresse senden?"},
                {"hi","प्राप्त ईमेल पते पर टोकन भेजें?"},
                {"fr","Envoyer un jeton à une adresse e-mail enregistrée?"},
                {"zh-cht","將保安編碼發送到註冊的電郵地址？"},
                {"zh-chs","将保安编码发送到注册的电邮地址？"},
                {"fi","Lähetetäänkö tunnus rekisteröityyn sähköpostiosoitteeseen?"},
                {"tr","Belirteç kayıtlı e-posta adresine gönderilsin mi?"},
                {"cs","Odeslat token na zaregistrovanou e-mailovou adresu?"},
                {"ja","登録済みのメールアドレスにトークンを送信しますか？"},
                {"es","¿Enviar token a la dirección de correo electrónico registrada?"},
                {"pt","Enviar token para o endereço de e-mail registrado?"},
                {"nl","Token verzenden naar geregistreerd e-mailadres?"},
                {"pt-br","Enviar token para o endereço de e-mail registrado?"},
                {"sv","Skicka token till registrerad e-postadress?"},
                {"ko","등록된 이메일 주소로 토큰을 보내시겠습니까?"},
                {"it","Invia token all'indirizzo email registrato?"},
                {"ru","Отправить токен на зарегистрированный адрес электронной почты?"}
            }
        },
        {
            "Device Status",
            new Dictionary<string, string>() {
                {"nl","Apparaatstatus"},
                {"ko","장치 상태"},
                {"fr","Statut du périphérique"},
                {"it","Stato del dispositivo"},
                {"zh-chs","设备状态"},
                {"pt-br","Status do dispositivo"},
                {"sv","Enhets status"},
                {"es","Estado del dispositivo"},
                {"hi","उपकरण की स्थिति"},
                {"de","Gerätestatus"}
            }
        },
        {
            "Send token to registered phone number?",
            new Dictionary<string, string>() {
                {"de","Token an registrierte Telefonnummer senden?"},
                {"hi","विनियमित फोन नंबर पर टोकन भेजें?"},
                {"fr","Envoyer un jeton au numéro de téléphone enregistré?"},
                {"zh-cht","將保安編碼發送到註冊電話號碼？"},
                {"zh-chs","将保安编码发送到注册电话号码？"},
                {"fi","Lähetetäänkö tunnus rekisteröityyn puhelinnumeroon?"},
                {"tr","Belirteç kayıtlı telefon numarasına gönderilsin mi?"},
                {"cs","Poslat token na registrované telefonní číslo?"},
                {"ja","登録した電話番号にトークンを送信しますか？"},
                {"es","¿Enviar token al número de teléfono registrado?"},
                {"pt","Enviar token para o número de telefone registrado?"},
                {"nl","Token naar geregistreerd telefoonnummer verzenden?"},
                {"pt-br","Enviar token para o número de telefone registrado?"},
                {"sv","Skicka token till registrerat telefonnummer?"},
                {"ko","등록된 휴대폰 번호로 토큰을 보내시겠습니까?"},
                {"it","Invia token al numero di telefono registrato?"},
                {"ru","Отправить токен на зарегистрированный номер телефона?"}
            }
        },
        {
            "Frame rate",
            new Dictionary<string, string>() {
                {"de","Bildrate"},
                {"hi","फ्रेम रेट"},
                {"fr","Taux de rafraîchissement"},
                {"zh-cht","框速率"},
                {"zh-chs","贞速率"},
                {"fi","Ruudunpäivitysnopeus"},
                {"tr","Kare hızı"},
                {"cs","Snímková frekvence"},
                {"ja","フレームレート"},
                {"es","Cuadros por segundo"},
                {"pt","Taxa de quadros"},
                {"nl","Frameverhouding"},
                {"pt-br","Taxa de quadros"},
                {"sv","Bildhastighet"},
                {"ko","프레임 속도"},
                {"it","Frequenza dei fotogrammi"},
                {"ru","Частота кадров"}
            }
        },
        {
            "Scaling",
            new Dictionary<string, string>() {
                {"de","Skalierung"},
                {"hi","स्केलिंग"},
                {"fr","Mise à l'échelle"},
                {"zh-cht","縮放比例"},
                {"zh-chs","缩放比例"},
                {"fi","Skaalaus"},
                {"tr","Ölçeklendirme"},
                {"cs","Škálování"},
                {"ja","スケーリング"},
                {"es","Escala"},
                {"pt","Dimensionamento"},
                {"nl","Schalen"},
                {"pt-br","Dimensionamento"},
                {"sv","Skalning"},
                {"ko","비율"},
                {"it","Ridimensionamento"},
                {"ru","Маштабирование"}
            }
        },
        {
            "UDP",
            new Dictionary<string, string>() {
                {"hi","यूडीपी"}
            }
        },
        {
            "Next",
            new Dictionary<string, string>() {
                {"nl","Volgende"},
                {"ko","다음"},
                {"fr","Suivant"},
                {"it","Prossimo"},
                {"zh-chs","下一个"},
                {"pt-br","Próximo"},
                {"sv","Nästa"},
                {"es","Próximo"},
                {"hi","अगला"},
                {"de","Nächster"}
            }
        },
        {
            "Use Remote Keyboard Map",
            new Dictionary<string, string>() {
                {"de","Entfernte Tastaturbelegung verwenden"},
                {"hi","रिमोट कीबोर्ड मैप का उपयोग करें"},
                {"fr","Utiliser la configuration du clavier distant"},
                {"zh-cht","使用遠程鍵盤映射"},
                {"zh-chs","使用远程键盘映射"},
                {"fi","Käytä kaukonäppäimistökarttaa"},
                {"tr","Uzak Klavye Haritasını Kullan"},
                {"cs","Použijte Mapu vzdálené klávesnice"},
                {"ja","リモートキーボードマップを使用する"},
                {"es","Usar Mapa de Teclado Remoto"},
                {"pt","Use o mapa do teclado remoto"},
                {"nl","Gebruik de externe toetsenbord instelling"},
                {"pt-br","Use o mapa do teclado remoto"},
                {"sv","Använd Remote Keyboard Map"},
                {"ko","원격 키보드 맵 사용"},
                {"it","Usa mappa tastiera remota "},
                {"ru","Использовать карту удаленной клавиатуры"}
            }
        },
        {
            "Stats",
            new Dictionary<string, string>() {
                {"de","Statistiken"},
                {"hi","आँकड़े"},
                {"fr","Statistiques"},
                {"zh-cht","統計"},
                {"zh-chs","统计"},
                {"fi","Tilastot"},
                {"tr","İstatistikler"},
                {"cs","Statistiky"},
                {"ja","統計"},
                {"es","Estadísticas"},
                {"pt","Estatísticas"},
                {"nl","Statistieken"},
                {"pt-br","Estatísticas"},
                {"sv","Statistik"},
                {"ko","통계"},
                {"ru","Статистика"}
            }
        },
        {
            "Enhanced keyboard capture",
            new Dictionary<string, string>() {
                {"nl","Verbeterde toetsenbordopname"},
                {"ko","향상된 키보드 캡처"},
                {"fr","Capture de clavier améliorée"},
                {"it","Acquisizione avanzata della tastiera"},
                {"zh-chs","增强的键盘捕获"},
                {"pt-br","Captura de teclado aprimorada"},
                {"sv","Förbättrad tangentbordsfångst"},
                {"es","Captura de teclado mejorada"},
                {"hi","उन्नत कीबोर्ड कैप्चर"},
                {"de","Verbesserte Tastaturerfassung"}
            }
        },
        {
            "MeshCentral Router Update",
            new Dictionary<string, string>() {
                {"ko","MeshCentral 라우터 업데이트"},
                {"fr","Mise à jour du routeur MeshCentral"},
                {"it","Aggiornamento del router MeshCentral"},
                {"zh-chs","MeshCentral 路由器更新"},
                {"pt-br","Atualização do roteador MeshCentral"},
                {"sv","MeshCentral routeruppdatering"},
                {"es","Actualización del enrutador MeshCentral"},
                {"hi","मेशसेंट्रल राउटर अपडेट"},
                {"de","MeshCentral Router-Update"}
            }
        },
        {
            "Denied",
            new Dictionary<string, string>() {
                {"de","Verweigert"},
                {"hi","निषेध"},
                {"fr","Refusée"},
                {"zh-cht","被拒絕"},
                {"zh-chs","被拒绝"},
                {"fi","Evätty"},
                {"tr","Reddedildi"},
                {"cs","Odepřeno"},
                {"ja","拒否されました"},
                {"es","Denegado"},
                {"pt","Negado"},
                {"nl","Geweigerd"},
                {"pt-br","Negado"},
                {"sv","Förnekad"},
                {"ko","거부"},
                {"it","Negato"},
                {"ru","Отказано"}
            }
        },
        {
            "ComputerName",
            new Dictionary<string, string>() {
                {"nl","Computernaam"},
                {"fr","Nom de l'ordinateur"},
                {"it","Nome del computer"},
                {"zh-chs","计算机名"},
                {"pt-br","Nome do computador"},
                {"sv","Datornamn"},
                {"es","Nombre de la Computadora"},
                {"hi","कंप्यूटर का नाम"},
                {"de","Computername"}
            }
        },
        {
            "Recursive Delete",
            new Dictionary<string, string>() {
                {"nl","Recursief verwijderen"},
                {"ko","재귀 삭제"},
                {"fr","Suppression récursive"},
                {"it","Cancellazione ricorsiva"},
                {"zh-chs","递归删除"},
                {"pt-br","Exclusão recursiva"},
                {"sv","Rekursivt Radera"},
                {"es","Eliminación Recursiva"},
                {"hi","पुनरावर्ती हटाएं"},
                {"de","Rekursives Löschen"}
            }
        },
        {
            "MeshCentral Router",
            new Dictionary<string, string>() {
                {"hi","मेश्चरल राउटर"},
                {"fr","Routeur MeshCentral"},
                {"zh-chs","MeshCentral路由器"},
                {"fi","MeshCentral Reititin"},
                {"tr","MeshCentral Yönlendirici"},
                {"ja","MeshCentralルーター"},
                {"es","Router de MeshCentral "},
                {"pt-br","Roteador MeshCentral"},
                {"ko","MeshCentral 라우터"},
                {"it","Router MeshCentral"},
                {"ru","MeshCentral Router "}
            }
        },
        {
            "Name",
            new Dictionary<string, string>() {
                {"hi","नाम"},
                {"fr","Nom"},
                {"zh-cht","名稱"},
                {"zh-chs","名称"},
                {"fi","Nimi"},
                {"tr","İsim"},
                {"cs","Jméno/název"},
                {"ja","名"},
                {"es","Nombre"},
                {"pt","Nome"},
                {"nl","Naam"},
                {"pt-br","Nome"},
                {"sv","Namn"},
                {"ko","이름"},
                {"it","Nome"},
                {"ru","Имя"}
            }
        },
        {
            "Remote Desktop Data",
            new Dictionary<string, string>() {
                {"nl","Extern bureaublad gegevens"},
                {"ko","원격 데스크톱 데이터"},
                {"fr","Données de bureau à distance"},
                {"it","Dati desktop remoto"},
                {"zh-chs","远程桌面数据"},
                {"pt-br","Dados da área de trabalho remota"},
                {"sv","Fjärrskrivbordsdata"},
                {"es","Datos de Escritorio Remoto"},
                {"hi","दूरस्थ डेस्कटॉप डेटा"},
                {"de","Remotedesktopdaten"}
            }
        },
        {
            "Port Mapping",
            new Dictionary<string, string>() {
                {"nl","Poorttoewijzing"},
                {"ko","포트 매핑"},
                {"fr","Mappage des ports"},
                {"it","Mappatura delle porte"},
                {"zh-chs","端口映射"},
                {"pt-br","Mapeamento de portas"},
                {"sv","Portmappning"},
                {"es","Mapeo de Puertos"},
                {"hi","पोर्ट मानचित्रण"},
                {"de","Port-Mapping"}
            }
        },
        {
            "Application Name",
            new Dictionary<string, string>() {
                {"nl","Naam van de toepassing"},
                {"ko","응용 프로그램 이름"},
                {"fr","Nom de l'application"},
                {"it","Nome dell'applicazione"},
                {"zh-chs","应用名称"},
                {"pt-br","Nome da Aplicação"},
                {"sv","applikationsnamn"},
                {"es","Nombre de la aplicación"},
                {"hi","आवेदन का नाम"},
                {"de","Anwendungsname"}
            }
        },
        {
            "Error Message",
            new Dictionary<string, string>() {
                {"nl","Foutmelding"},
                {"ko","에러 메시지"},
                {"fr","Message d'erreur"},
                {"it","Messaggio di errore"},
                {"zh-chs","错误信息"},
                {"pt-br","Mensagem de erro"},
                {"sv","Felmeddelande"},
                {"es","Mensaje de error"},
                {"hi","त्रुटि संदेश"},
                {"de","Fehlermeldung"}
            }
        },
        {
            "Path",
            new Dictionary<string, string>() {
                {"nl","Pad"},
                {"ko","통로"},
                {"fr","Chemin"},
                {"it","Percorso"},
                {"zh-chs","小路"},
                {"pt-br","Caminho"},
                {"sv","Väg"},
                {"es","Camino"},
                {"hi","पथ"},
                {"de","Pfad"}
            }
        },
        {
            "Token",
            new Dictionary<string, string>() {
                {"de","Zeichen"},
                {"hi","टोकन"},
                {"fr","Jeton"},
                {"zh-cht","代幣"},
                {"zh-chs","代币"},
                {"fi","Tunnus"},
                {"tr","Jeton"},
                {"cs","Žeton"},
                {"ja","トークン"},
                {"pt","Símbolo"},
                {"pt-br","Símbolo"},
                {"sv","Tecken"},
                {"ko","토큰"},
                {"it","Gettone"},
                {"ru","Токен"}
            }
        },
        {
            " MeshCentral Router",
            new Dictionary<string, string>() {
                {"ko"," MeshCentral 라우터"},
                {"fr"," Routeur MeshCentral"},
                {"it","MeshCentral Router"},
                {"zh-chs"," MeshCentral 路由器"},
                {"pt-br"," Roteador MeshCentral"},
                {"es"," Enrutador MeshCentral"},
                {"hi"," मेशसेंट्रल राउटर"},
                {"de"," MeshCentral-Router"}
            }
        },
        {
            "Swap Mouse Buttons",
            new Dictionary<string, string>() {
                {"de","Maustasten vertauschen"},
                {"hi","माउस माउस को स्वैप करें"},
                {"fr","Echanger les boutons de la souris"},
                {"zh-cht","交換鼠標按鈕"},
                {"zh-chs","交换鼠标按钮"},
                {"fi","Vaihda hiiren painikkeet"},
                {"tr","Fare Düğmelerini Değiştirin"},
                {"cs","Zaměňte tlačítka myši"},
                {"ja","マウスボタンを交換する"},
                {"es","Cambiar Botones del Mouse"},
                {"pt","Botões de troca do mouse"},
                {"nl","Wissel muisknoppen"},
                {"pt-br","Botões de troca do mouse"},
                {"sv","Byt musknappar"},
                {"ko","마우스 버튼 교체"},
                {"it","Scambia i pulsanti del mouse"},
                {"ru","Поменять местами кнопки мыши"}
            }
        },
        {
            "Change remote desktop settings",
            new Dictionary<string, string>() {
                {"nl","Instellingen voor extern bureaublad wijzigen"},
                {"ko","원격 데스크톱 설정 변경"},
                {"fr","Modifier les paramètres du bureau à distance"},
                {"it","Modifica le impostazioni del desktop remoto"},
                {"zh-chs","更改远程桌面设置"},
                {"pt-br","Alterar as configurações da área de trabalho remota"},
                {"sv","Ändra fjärrskrivbordsinställningar"},
                {"es","Cambiar la configuración del escritorio remoto"},
                {"hi","दूरस्थ डेस्कटॉप सेटिंग बदलें"},
                {"de","Remote-Desktop-Einstellungen ändern"}
            }
        },
        {
            "Relay",
            new Dictionary<string, string>() {
                {"de","Weiterleitung"},
                {"hi","रिले"},
                {"fr","Relais"},
                {"zh-cht","中繼"},
                {"zh-chs","中继"},
                {"fi","Rele"},
                {"tr","Röle"},
                {"cs","Předávání (relay)"},
                {"ja","リレー"},
                {"es","Relé"},
                {"pt","Retransmissão"},
                {"pt-br","Retransmissão"},
                {"sv","Relä"},
                {"ko","전달(Relay)"},
                {"it","Ritrasmissioni"},
                {"ru","Ретранслятор"}
            }
        },
        {
            "Remote Desktop Stats",
            new Dictionary<string, string>() {
                {"nl","Extern bureaublad statistieken"},
                {"ko","원격 데스크톱 통계"},
                {"fr","Statistiques du bureau à distance"},
                {"it","Statistiche desktop remoto"},
                {"zh-chs","远程桌面统计"},
                {"pt-br","Estatísticas da área de trabalho remota"},
                {"sv","Statistik för fjärrskrivbord"},
                {"es","Estadísticas de Escritorio Remoto"},
                {"hi","दूरस्थ डेस्कटॉप आँकड़े"},
                {"de","Remotedesktop-Statistiken"}
            }
        },
        {
            "Alternative Port",
            new Dictionary<string, string>() {
                {"nl","Alternatieve poort"},
                {"ko","대체 포트"},
                {"fr","Port alternatif"},
                {"it","Porta alternativa"},
                {"zh-chs","替代端口"},
                {"pt-br","Porto Alternativo"},
                {"sv","Alternativ hamn"},
                {"es","Puerto Alternativo"},
                {"hi","वैकल्पिक बंदरगाह"},
                {"de","Alternativer Hafen"}
            }
        },
        {
            "Toggle zoom-to-fit mode",
            new Dictionary<string, string>() {
                {"nl","Schakel inzoemen naar passend modus in"},
                {"ko","확대 / 축소 모드 전환"},
                {"fr","Basculer en mode zoom pour ajuster"},
                {"it","Attiva/disattiva la modalità zoom per adattare"},
                {"zh-chs","切换缩放至适合模式"},
                {"pt-br","Alternar modo de zoom para ajustar"},
                {"sv","Växla zoom-to-fit-läge"},
                {"es","Alternar el modo de zoom para ajustar"},
                {"hi","ज़ूम-टू-फ़िट मोड टॉगल करें"},
                {"de","Zoom-to-Fit-Modus umschalten"}
            }
        },
        {
            "Protocol",
            new Dictionary<string, string>() {
                {"de","Protokoll"},
                {"hi","मसविदा बनाना"},
                {"fr","Protocole"},
                {"zh-cht","協議"},
                {"zh-chs","协议"},
                {"fi","Protokolla"},
                {"tr","Protokol"},
                {"cs","Protokol"},
                {"ja","プロトコル"},
                {"es","Protocolo"},
                {"pt","Protocolo"},
                {"pt-br","Protocolo"},
                {"sv","Protokoll"},
                {"ko","프로토콜"},
                {"it","Protocollo"},
                {"ru","Протокол"}
            }
        },
        {
            "Send Ctrl-Alt-Del to remote device",
            new Dictionary<string, string>() {
                {"nl","Stuur Ctrl-Alt-Del naar extern apparaat"},
                {"ko","Ctrl-Alt-Del을 원격 장치로 보내기"},
                {"fr","Envoyer Ctrl-Alt-Suppr à l'appareil distant"},
                {"it","Invia Ctrl-Alt-Canc al dispositivo remoto"},
                {"zh-chs","发送 Ctrl-Alt-Del 到远程设备"},
                {"pt-br","Envie Ctrl-Alt-Del para o dispositivo remoto"},
                {"sv","Skicka Ctrl-Alt-Del till fjärrenhet"},
                {"es","Enviar Ctrl-Alt-Del al dispositivo remoto"},
                {"hi","रिमोट डिवाइस पर Ctrl-Alt-Del भेजें"},
                {"de","Strg-Alt-Entf an Remote-Gerät senden"}
            }
        },
        {
            "Sort by G&roup",
            new Dictionary<string, string>() {
                {"nl","Sorteer op G&roep"},
                {"ko","그룹 별 정렬 (& R)"},
                {"fr","Trier par groupe"},
                {"it","Ordina per gruppo"},
                {"zh-chs","按组(&O) 排序"},
                {"pt-br","Classificar por G & Rupo"},
                {"sv","Sortera efter G & roup"},
                {"es","Ordenar por grupo y grupo"},
                {"hi","समूह के आधार पर छाँटें"},
                {"de","Nach Gruppe sortieren"}
            }
        },
        {
            "Remove 1 item?",
            new Dictionary<string, string>() {
                {"nl","1 artikel verwijderen?"},
                {"ko","항목 1 개를 삭제 하시겠습니까?"},
                {"fr","Supprimer 1 élément ?"},
                {"it","Rimuovere 1 elemento?"},
                {"zh-chs","删除 1 项？"},
                {"pt-br","Remover 1 item?"},
                {"sv","Ta bort ett objekt?"},
                {"es","¿Eliminar 1 artículo?"},
                {"hi","1 आइटम निकालें?"},
                {"de","1 Artikel entfernen?"}
            }
        },
        {
            "Local",
            new Dictionary<string, string>() {
                {"de","Lokal"},
                {"hi","स्थानीय"},
                {"zh-cht","本地"},
                {"zh-chs","本地"},
                {"fi","Paikallinen"},
                {"tr","Yerel"},
                {"cs","Lokální"},
                {"ja","地元"},
                {"nl","Lokaal"},
                {"sv","Lokal"},
                {"ko","로컬"},
                {"it","Locale"},
                {"ru","Локальный"}
            }
        },
        {
            "Unable to bind to local port",
            new Dictionary<string, string>() {
                {"nl","Kan niet binden aan lokale poort"},
                {"ko","로컬 포트에 바인딩 할 수 없습니다."},
                {"fr","Impossible de se lier au port local"},
                {"it","Impossibile eseguire il binding alla porta locale"},
                {"zh-chs","无法绑定到本地端口"},
                {"pt-br","Incapaz de ligar à porta local"},
                {"sv","Det gick inte att binda till lokal port"},
                {"es","No se puede vincular al puerto local"},
                {"hi","स्थानीय पोर्ट से जुड़ने में असमर्थ"},
                {"de","Kann nicht an lokalen Port binden"}
            }
        },
        {
            "Outgoing Bytes",
            new Dictionary<string, string>() {
                {"nl","Uitgaande Bytes"},
                {"ko","나가는 바이트"},
                {"fr","Octets sortants"},
                {"it","Byte in uscita"},
                {"zh-chs","传出字节"},
                {"pt-br","Bytes de saída"},
                {"sv","Utgående byte"},
                {"es","Bytes Salientes"},
                {"hi","आउटगोइंग बाइट्स"},
                {"de","Ausgehende Bytes"}
            }
        },
        {
            "Open...",
            new Dictionary<string, string>() {
                {"ko","열다..."},
                {"fr","Ouvert..."},
                {"it","Aperto..."},
                {"zh-chs","打开..."},
                {"pt-br","Aberto..."},
                {"sv","Öppna..."},
                {"es","Abierto..."},
                {"hi","खुला हुआ..."},
                {"de","Öffnen..."}
            }
        },
        {
            "Languages",
            new Dictionary<string, string>() {
                {"nl","Talen"},
                {"ko","언어"},
                {"fr","Langues"},
                {"it","Linguaggi"},
                {"zh-chs","语言"},
                {"pt-br","línguas"},
                {"sv","språk"},
                {"es","Idiomas"},
                {"hi","बोली"},
                {"de","Sprachen"}
            }
        },
        {
            "S&ettings...",
            new Dictionary<string, string>() {
                {"nl","Instellingen..."},
                {"ko","설정 (& E) ..."},
                {"fr","Paramètres..."},
                {"zh-chs","设置(&E)..."},
                {"pt-br","Configurações..."},
                {"sv","Inställningar..."},
                {"es","A&justes..."},
                {"hi","समायोजन..."},
                {"de","Die Einstellungen..."}
            }
        },
        {
            "Port Mapping Help",
            new Dictionary<string, string>() {
                {"nl","Hulp bij poorttoewijzing"},
                {"ko","포트 매핑 도움말"},
                {"fr","Aide sur le mappage de ports"},
                {"it","Aiuto per la mappatura delle porte"},
                {"zh-chs","端口映射帮助"},
                {"pt-br","Ajuda para mapeamento de portas"},
                {"sv","Portmappning Hjälp"},
                {"es","Ayuda de Mapeo de Puertos"},
                {"hi","पोर्ट मैपिंग सहायता"},
                {"de","Hilfe zur Portzuordnung"}
            }
        },
        {
            "Very slow",
            new Dictionary<string, string>() {
                {"de","Sehr langsam"},
                {"hi","बहुत धीमी गति से"},
                {"fr","Très lent"},
                {"zh-cht","非常慢"},
                {"zh-chs","非常慢"},
                {"fi","Hyvin hidas"},
                {"tr","Çok yavaş"},
                {"cs","Velmi pomalu"},
                {"ja","非常に遅い"},
                {"es","Muy lento"},
                {"pt","Muito devagar"},
                {"nl","Erg traag"},
                {"pt-br","Muito devagar"},
                {"sv","Väldigt långsam"},
                {"ko","아주 느린"},
                {"it","Molto lento"},
                {"ru","Очень медленно"}
            }
        },
        {
            "WARNING - Invalid Server Certificate",
            new Dictionary<string, string>() {
                {"nl","WAARSCHUWING - Ongeldig servercertificaat"},
                {"ko","경고-잘못된 서버 인증서"},
                {"fr","AVERTISSEMENT - Certificat de serveur non valide"},
                {"it","AVVISO - Certificato del server non valido"},
                {"zh-chs","警告 - 服务器证书无效"},
                {"pt-br","AVISO - Certificado de servidor inválido"},
                {"sv","VARNING - Ogiltigt servercertifikat"},
                {"es","ADVERTENCIA: Certificado de Servidor No Válido"},
                {"hi","चेतावनी - अमान्य सर्वर प्रमाणपत्र"},
                {"de","WARNUNG - Ungültiges Serverzertifikat"}
            }
        },
        {
            "Setup...",
            new Dictionary<string, string>() {
                {"de","Aufbau..."},
                {"hi","सेट अप..."},
                {"fr","Traitement..."},
                {"zh-cht","設定..."},
                {"zh-chs","设定..."},
                {"fi","Asennus..."},
                {"tr","Kurmak..."},
                {"cs","Nastavení…"},
                {"ja","セットアップ..."},
                {"es","Configurar..."},
                {"pt","Configurando..."},
                {"pt-br","Configurar..."},
                {"sv","Uppstart..."},
                {"ko","설치..."},
                {"it","Impostare..."},
                {"ru","Установка..."}
            }
        },
        {
            "Remember this certificate",
            new Dictionary<string, string>() {
                {"nl","Onthoud dit certificaat"},
                {"ko","이 인증서 기억"},
                {"fr","Rappelez-vous ce certificat"},
                {"it","Ricorda questo certificato"},
                {"zh-chs","记住这个证书"},
                {"pt-br","Lembre-se deste certificado"},
                {"sv","Kom ihåg detta certifikat"},
                {"es","Recuerda este certificado"},
                {"hi","यह प्रमाणपत्र याद रखें"},
                {"de","Merken Sie sich dieses Zertifikat"}
            }
        },
        {
            "RDP Port",
            new Dictionary<string, string>() {
                {"nl","RDP poort"},
                {"ko","RDP 포트"},
                {"fr","Port RDP"},
                {"it","Porta RDP"},
                {"zh-chs","RDP 端口"},
                {"pt-br","Porta RDP"},
                {"sv","RDP-port"},
                {"es","Puerto RDP"},
                {"hi","आरडीपी पोर्ट"},
                {"de","RDP-Port"}
            }
        },
        {
            "Slow",
            new Dictionary<string, string>() {
                {"de","Langsam"},
                {"hi","धीरे"},
                {"fr","Lent"},
                {"zh-cht","慢"},
                {"zh-chs","慢"},
                {"fi","Hidas"},
                {"tr","Yavaş"},
                {"cs","Pomalu"},
                {"ja","スロー"},
                {"es","Lento"},
                {"pt","Lento"},
                {"nl","Traag"},
                {"pt-br","Lento"},
                {"sv","Långsam"},
                {"ko","느린"},
                {"it","Lento"},
                {"ru","Медленно"}
            }
        },
        {
            "Pull clipboard from remote device",
            new Dictionary<string, string>() {
                {"nl","Trek het klembord van het externe apparaat"},
                {"ko","원격 장치에서 클립 보드 가져 오기"},
                {"fr","Extraire le presse-papiers de l'appareil distant"},
                {"it","Estrai gli appunti dal dispositivo remoto"},
                {"zh-chs","从远程设备拉剪贴板"},
                {"pt-br","Puxe a área de transferência do dispositivo remoto"},
                {"sv","Dra urklipp från fjärrenheten"},
                {"es","Extraer el Portapapeles del dispositivo remoto"},
                {"hi","दूरस्थ डिवाइस से क्लिपबोर्ड खींचे"},
                {"de","Zwischenablage von Remote-Gerät ziehen"}
            }
        },
        {
            "Remote",
            new Dictionary<string, string>() {
                {"de","Entfernt"},
                {"hi","रिमोट"},
                {"fr","Éloigné"},
                {"zh-cht","遠程"},
                {"zh-chs","远程"},
                {"fi","Etä"},
                {"tr","Uzaktan kumanda"},
                {"cs","Vzdálené"},
                {"ja","リモート"},
                {"es","Remoto"},
                {"pt","Remoto"},
                {"nl","Extern"},
                {"pt-br","Controlo remoto"},
                {"sv","Avlägsen"},
                {"ko","원격"},
                {"it","Remoto"},
                {"ru","Удаленно"}
            }
        },
        {
            "TCP",
            new Dictionary<string, string>() {
                {"hi","टीसीपी"}
            }
        },
        {
            "Settings",
            new Dictionary<string, string>() {
                {"de","Einstellungen"},
                {"hi","समायोजन"},
                {"fr","Paramètres"},
                {"zh-cht","設定"},
                {"zh-chs","设定"},
                {"fi","Asetukset"},
                {"tr","Ayarlar"},
                {"cs","Nastavení"},
                {"ja","設定"},
                {"es","Opciones"},
                {"pt","Configurações"},
                {"nl","Instellingen"},
                {"pt-br","Configurações"},
                {"sv","inställningar"},
                {"ko","설정"},
                {"it","impostazioni"},
                {"ru","Настройки"}
            }
        },
        {
            "MeshCentral Router allows mapping of TCP and UDP ports on this computer to any computer in your MeshCentral server account. Start by logging into your account.",
            new Dictionary<string, string>() {
                {"nl","Met MeshCentral Router kunnen TCP en UDP poorten op deze computer worden toegewezen aan elke computer in uw MeshCentral-serveraccount. Begin door in te loggen op uw account."},
                {"ko","MeshCentral 라우터를 사용하면이 컴퓨터의 TCP 및 UDP 포트를 MeshCentral 서버 계정의 모든 컴퓨터에 매핑 할 수 있습니다. 계정에 로그인하여 시작하십시오."},
                {"fr","Le routeur MeshCentral permet de mapper les ports TCP et UDP de cet ordinateur sur n'importe quel ordinateur de votre compte de serveur MeshCentral. Commencez par vous connecter à votre compte."},
                {"it","MeshCentral Router consente la mappatura delle porte TCP e UDP su questo computer su qualsiasi computer nell'account del server MeshCentral. Inizia accedendo al tuo account."},
                {"zh-chs","MeshCentral 路由器允许将此计算机上的 TCP 和 UDP 端口映射到您的 MeshCentral 服务器帐户中的任何计算机。首先登录您的帐户。"},
                {"pt-br","O roteador MeshCentral permite o mapeamento das portas TCP e UDP neste computador para qualquer computador em sua conta do servidor MeshCentral. Comece fazendo login em sua conta."},
                {"sv","MeshCentral Router tillåter mappning av TCP- och UDP-portar på den här datorn till vilken dator som helst i ditt MeshCentral-serverkonto. Börja med att logga in på ditt konto."},
                {"es","MeshCentral Router permite la asignación de puertos TCP y UDP en esta computadora a cualquier computadora en tu cuenta de servidor MeshCentral. Empieza por iniciar sesión en tu cuenta."},
                {"hi","MeshCentral राउटर इस कंप्यूटर पर आपके MeshCentral सर्वर खाते के किसी भी कंप्यूटर पर TCP और UDP पोर्ट की मैपिंग की अनुमति देता है। अपने खाते में लॉग इन करके प्रारंभ करें।"},
                {"de","MeshCentral Router ermöglicht die Zuordnung von TCP- und UDP-Ports auf diesem Computer zu jedem Computer in Ihrem MeshCentral-Serverkonto. Melden Sie sich zunächst bei Ihrem Konto an."}
            }
        },
        {
            "Outgoing Compression",
            new Dictionary<string, string>() {
                {"nl","Uitgaande compressie"},
                {"ko","나가는 압축"},
                {"fr","Compression sortante"},
                {"it","Compressione in uscita"},
                {"zh-chs","输出压缩"},
                {"pt-br","Compressão de saída"},
                {"sv","Utgående kompression"},
                {"es","Compresión Saliente"},
                {"hi","आउटगोइंग संपीड़न"},
                {"de","Ausgehende Komprimierung"}
            }
        },
        {
            ", {0} users",
            new Dictionary<string, string>() {
                {"nl",", {0} gebruikers"},
                {"ko",", {0} 명의 사용자"},
                {"fr",", {0} utilisateurs"},
                {"it",", {0} utenti"},
                {"zh-chs",", {0} 个用户"},
                {"pt-br",", {0} usuários"},
                {"sv",", {0} användare"},
                {"es",", {0} usuarios"},
                {"hi",", {0} उपयोगकर्ता"},
                {"de",", {0} Nutzer"}
            }
        },
        {
            "This MeshCentral Server uses a different version of this tool. Click ok to download and update.",
            new Dictionary<string, string>() {
                {"nl","Deze MeshCentral Server gebruikt een andere versie van deze tool. Klik op OK om te downloaden en bij te werken."},
                {"ko","이 MeshCentral 서버는이 도구의 다른 버전을 사용합니다. 확인을 클릭하여 다운로드하고 업데이트하십시오."},
                {"fr","Ce serveur MeshCentral utilise une version différente de cet outil. Cliquez sur ok pour télécharger et mettre à jour."},
                {"it","Questo server MeshCentral utilizza una versione diversa di questo strumento.Fare clic su OK per scaricare e aggiornare."},
                {"zh-chs","此 MeshCentral Server 使用此工具的不同版本。单击“确定”进行下载和更新。"},
                {"pt-br","Este servidor MeshCentral usa uma versão diferente desta ferramenta. Clique em ok para baixar e atualizar."},
                {"sv","Denna MeshCentral Server använder en annan version av detta verktyg. Klicka på ok för att ladda ner och uppdatera."},
                {"es","Este servidor MeshCentral utiliza una versión diferente de esta herramienta. Haz clic en Aceptar para descargar y actualizar."},
                {"hi","यह MeshCentral सर्वर इस टूल के भिन्न संस्करण का उपयोग करता है। डाउनलोड और अपडेट करने के लिए ओके पर क्लिक करें।"},
                {"de","Dieser MeshCentral Server verwendet eine andere Version dieses Tools. Klicken Sie auf OK, um herunterzuladen und zu aktualisieren."}
            }
        },
        {
            "Cancel Auto-Close",
            new Dictionary<string, string>() {
                {"nl","Automatisch sluiten annuleren"},
                {"ko","자동 닫기 취소"},
                {"fr","Annuler la fermeture automatique"},
                {"it","Annulla chiusura automatica"},
                {"zh-chs","取消自动关闭"},
                {"pt-br","Cancelar fechamento automático"},
                {"sv","Avbryt automatisk stängning"},
                {"es","Cancelar Cierre Automático"},
                {"hi","रद्द करें स्वतः बंद"},
                {"de","Automatisches Schließen abbrechen"}
            }
        },
        {
            "Open Source, Apache 2.0 License",
            new Dictionary<string, string>() {
                {"nl","Open Source, Apache 2.0 Licentie"},
                {"ko","오픈 소스, Apache 2.0 라이선스"},
                {"fr","Open Source, licence Apache 2.0"},
                {"it","Open Source, licenza Apache 2.0"},
                {"zh-chs","开源，Apache 2.0 许可"},
                {"pt-br","Código aberto, licença Apache 2.0"},
                {"sv","Open Source, Apache 2.0-licens"},
                {"es","Código Abierto, licencia Apache 2.0"},
                {"hi","ओपन सोर्स, अपाचे 2.0 लाइसेंस"},
                {"de","Open Source, Apache 2.0-Lizenz"}
            }
        },
        {
            "Mappings",
            new Dictionary<string, string>() {
                {"nl","Toewijzingen"},
                {"ko","매핑"},
                {"fr","Mappages"},
                {"it","Mappature"},
                {"zh-chs","映射"},
                {"pt-br","Mapeamentos"},
                {"sv","Kartläggningar"},
                {"es","Mapeos"},
                {"hi","मानचित्रण"},
                {"de","Zuordnungen"}
            }
        },
        {
            "Devices",
            new Dictionary<string, string>() {
                {"de","Geräte"},
                {"hi","उपकरण"},
                {"fr","Dispositifs"},
                {"zh-cht","裝置"},
                {"zh-chs","设备"},
                {"fi","Laitteet"},
                {"tr","Cihazlar"},
                {"cs","Zařízení"},
                {"ja","デバイス"},
                {"es","Dispositivos"},
                {"pt","Dispositivos"},
                {"nl","Apparaten"},
                {"pt-br","Dispositivos"},
                {"sv","Enheter"},
                {"ko","여러 장치"},
                {"it","Dispositivi"},
                {"ru","Устройства"}
            }
        },
        {
            "(Individual Devices)",
            new Dictionary<string, string>() {
                {"nl","(Individuele apparaten)"},
                {"ko","(개별 기기)"},
                {"fr","(Appareils individuels)"},
                {"it","(Dispositivi individuali)"},
                {"zh-chs","（个别设备）"},
                {"pt-br","(Dispositivos Individuais)"},
                {"sv","(Enskilda enheter)"},
                {"es","(Dispositivos individuales)"},
                {"hi","(व्यक्तिगत उपकरण)"},
                {"de","(Einzelgeräte)"}
            }
        },
        {
            "Privacy Bar",
            new Dictionary<string, string>() {
                {"de","Datenschutzleiste"},
                {"hi","गोपनीयता बार"},
                {"fr","Barre de confidentialité"},
                {"zh-cht","隱私欄"},
                {"zh-chs","隐私栏"},
                {"fi","Tietosuojapalkki"},
                {"tr","Gizlilik Çubuğu"},
                {"cs","Bar ochrany osobních údajů"},
                {"ja","プライバシーバー"},
                {"es","Barra de Privacidad"},
                {"pt","Barra de Privacidade"},
                {"nl","Privacy balk"},
                {"pt-br","Barra de Privacidade"},
                {"sv","Sekretessfält"},
                {"ko","프라이버시 바"},
                {"it","Privacy bar"},
                {"ru","Панель конфиденциальности"}
            }
        },
        {
            "Remove {0} items?",
            new Dictionary<string, string>() {
                {"nl","{0} items verwijderen?"},
                {"ko","{0} 개 항목을 삭제 하시겠습니까?"},
                {"fr","Supprimer {0} éléments ?"},
                {"it","Rimuovere {0} elementi?"},
                {"zh-chs","删除 {0} 项？"},
                {"pt-br","Remover {0} itens?"},
                {"sv","Ta bort {0} objekt?"},
                {"es","¿Eliminar {0} elementos?"},
                {"hi","{0} आइटम निकालें?"},
                {"de","{0} Elemente entfernen?"}
            }
        },
        {
            "This server presented a un-trusted certificate.  This may indicate that this is not the correct server or that the server does not have a valid certificate. It is not recommanded, but you can press the ignore button to continue connection to this server.",
            new Dictionary<string, string>() {
                {"nl","Deze server heeft een niet-vertrouwd certificaat gepresenteerd. Dit kan erop wijzen dat dit niet de juiste server is of dat de server geen geldig certificaat heeft. Het wordt niet aanbevolen, maar u kunt op de negeren drukken om de verbinding met deze server voort te zetten."},
                {"ko","이 서버는 신뢰할 수없는 인증서를 제공했습니다. 이는 올바른 서버가 아니거나 서버에 유효한 인증서가 없음을 나타낼 수 있습니다. 권장되지는 않지만 무시 버튼을 눌러이 서버에 계속 연결할 수 있습니다."},
                {"fr","Ce serveur a présenté un certificat non approuvé. Cela peut indiquer qu'il ne s'agit pas du bon serveur ou que le serveur n'a pas de certificat valide. Ce n'est pas recommandé, mais vous pouvez appuyer sur le bouton ignorer pour continuer la connexion à ce serveur."},
                {"it","Questo server ha presentato un certificato non attendibile.Questo potrebbe indicare che questo non è il server corretto o che il server non ha un certificato valido.Non è raccomandato, ma puoi premere il pulsante Ignora per continuare la connessione a questo server."},
                {"zh-chs","此服务器提供了不受信任的证书。这可能表明这不是正确的服务器或服务器没有有效的证书。不推荐，但您可以按忽略按钮继续连接到此服务器。"},
                {"pt-br","Este servidor apresentou um certificado não confiável. Isso pode indicar que este não é o servidor correto ou que o servidor não possui um certificado válido. Não é recomendado, mas você pode pressionar o botão de ignorar para continuar a conexão com este servidor."},
                {"sv","Denna server presenterade ett otillförlitligt certifikat. Detta kan indikera att detta inte är rätt server eller att servern inte har ett giltigt certifikat. Det rekommenderas inte, men du kan trycka på ignorera-knappen för att fortsätta anslutningen till den här servern."},
                {"es","Este servidor presentó un certificado no confiable. Esto puede indicar que este no es el servidor correcto o que el servidor no tiene un certificado válido. No se recomienda, pero puedes presionar el botón ignorar para continuar la conexión a este servidor."},
                {"hi","इस सर्वर ने एक अविश्वसनीय प्रमाणपत्र प्रस्तुत किया। यह संकेत दे सकता है कि यह सही सर्वर नहीं है या सर्वर के पास वैध प्रमाणपत्र नहीं है। यह अनुशंसित नहीं है, लेकिन आप इस सर्वर से कनेक्शन जारी रखने के लिए अनदेखा करें बटन दबा सकते हैं।"},
                {"de","Dieser Server hat ein nicht vertrauenswürdiges Zertifikat vorgelegt. Dies kann darauf hinweisen, dass dies nicht der richtige Server ist oder dass der Server nicht über ein gültiges Zertifikat verfügt. Es wird nicht empfohlen, aber Sie können die Ignorieren-Taste drücken, um die Verbindung zu diesem Server fortzusetzen."}
            }
        },
        {
            "Password",
            new Dictionary<string, string>() {
                {"de","Passwort"},
                {"hi","कुंजिका"},
                {"fr","Mot de passe"},
                {"zh-cht","密碼"},
                {"zh-chs","密码"},
                {"fi","Salasana"},
                {"tr","Parola"},
                {"cs","Heslo"},
                {"ja","パスワード"},
                {"es","Contraseña"},
                {"pt","Senha"},
                {"nl","Wachtwoord"},
                {"pt-br","Senha"},
                {"sv","Lösenord"},
                {"ko","암호"},
                {"ru","Пароль"}
            }
        },
        {
            "Login",
            new Dictionary<string, string>() {
                {"de","Anmeldung"},
                {"hi","लॉग इन करें"},
                {"fr","Connexion"},
                {"zh-cht","登入"},
                {"zh-chs","登录"},
                {"fi","Kirjaudu sisään"},
                {"tr","Oturum aç"},
                {"cs","Přihlášení"},
                {"ja","ログイン"},
                {"es","Iniciar sesión"},
                {"pt","Entrar"},
                {"nl","Inloggen"},
                {"pt-br","Conecte-se"},
                {"sv","Logga in"},
                {"ko","로그인"},
                {"ru","Войти"}
            }
        },
        {
            "&Save Mappings...",
            new Dictionary<string, string>() {
                {"nl","&Toewijzingen opslaan..."},
                {"ko","매핑 저장 ..."},
                {"fr","&Enregistrer les mappages..."},
                {"it","&Salva mappature..."},
                {"zh-chs","保存映射(&S)..."},
                {"pt-br","& Salvar mapeamentos ..."},
                {"sv","& Spara kartor ..."},
                {"es","&Guardar asignaciones ..."},
                {"hi","&मैपिंग सहेजें..."},
                {"de","&Zuordnungen speichern..."}
            }
        },
        {
            "All Displays",
            new Dictionary<string, string>() {
                {"de","Alle Bildschirme"},
                {"hi","सभी प्रदर्शित करता है"},
                {"fr","Tous affichages"},
                {"zh-cht","所有顯示"},
                {"zh-chs","所有显示"},
                {"fi","Kaikki näytöt"},
                {"tr","Tüm Ekranlar"},
                {"cs","Všechny displeje"},
                {"ja","すべてのディスプレイ"},
                {"es","Todas las Pantallas"},
                {"pt","Todas as telas"},
                {"nl","Alle schermen"},
                {"pt-br","Todos os monitores"},
                {"sv","Alla skärmar"},
                {"ko","모든 디스플레이"},
                {"it","Tutti i display"},
                {"ru","Все экраны"}
            }
        },
        {
            "Double Click Action",
            new Dictionary<string, string>() {
                {"nl","Dubbelklik actie"},
                {"ko","더블 클릭 동작"},
                {"fr","Action de double-clic"},
                {"it","Azione doppio clic"},
                {"zh-chs","双击操作"},
                {"pt-br","Ação de duplo clique"},
                {"sv","Dubbelklicka på åtgärd"},
                {"es","Acción de doble clic"},
                {"hi","डबल क्लिक एक्शन"},
                {"de","Doppelklick-Aktion"}
            }
        },
        {
            "Toggle remote desktop connection",
            new Dictionary<string, string>() {
                {"nl","Verbinding met extern bureaublad wisselen"},
                {"ko","원격 데스크톱 연결 전환"},
                {"fr","Basculer la connexion au bureau à distance"},
                {"it","Attiva/disattiva connessione desktop remoto"},
                {"zh-chs","切换远程桌面连接"},
                {"pt-br","Alternar conexão de área de trabalho remota"},
                {"sv","Växla fjärrskrivbordsanslutning"},
                {"es","Alternar la conexión de escritorio remoto"},
                {"hi","दूरस्थ डेस्कटॉप कनेक्शन टॉगल करें"},
                {"de","Remotedesktopverbindung umschalten"}
            }
        },
        {
            "Connect",
            new Dictionary<string, string>() {
                {"de","Verbinden"},
                {"hi","जुडिये"},
                {"fr","Se connecter"},
                {"zh-cht","連接"},
                {"zh-chs","连接"},
                {"fi","Yhdistä"},
                {"tr","Bağlan"},
                {"cs","Připojit"},
                {"ja","つなぐ"},
                {"es","Conectar"},
                {"pt","Conectar"},
                {"nl","Verbinden"},
                {"pt-br","Conectar"},
                {"sv","Anslut"},
                {"ko","연결"},
                {"it","Connetti"},
                {"ru","Подключиться"}
            }
        },
        {
            "CIRA",
            new Dictionary<string, string>() {
                {"hi","सीआईआरए"}
            }
        },
        {
            "Mapping Settings",
            new Dictionary<string, string>() {
                {"nl","Kaartinstellingen"},
                {"ko","매핑 설정"},
                {"fr","Paramètres de mappage"},
                {"it","Impostazioni di mappatura"},
                {"zh-chs","映射设置"},
                {"pt-br","Configurações de mapeamento"},
                {"sv","Kartläggningsinställningar"},
                {"es","Configuración de Mapeo"},
                {"hi","मैपिंग सेटिंग्स"},
                {"de","Zuordnungseinstellungen"}
            }
        },
        {
            ", 1 connection.",
            new Dictionary<string, string>() {
                {"nl",", 1 verbinding."},
                {"ko",", 1 촌."},
                {"fr",", 1 connexion."},
                {"it",", 1 connessione."},
                {"zh-chs",", 1 个连接。"},
                {"pt-br",", 1 conexão."},
                {"sv",", 1 anslutning."},
                {"es",", 1 conexión."},
                {"hi",", 1 कनेक्शन।"},
                {"de",", 1 Verbindung."}
            }
        },
        {
            "HTTPS",
            new Dictionary<string, string>() {
                {"hi","HTTPS के"}
            }
        },
        {
            "Install...",
            new Dictionary<string, string>() {
                {"nl","Installeren..."},
                {"ko","설치..."},
                {"fr","Installer..."},
                {"it","Installare..."},
                {"zh-chs","安装..."},
                {"pt-br","Instalar..."},
                {"sv","Installera..."},
                {"es","Instalar en pc..."},
                {"hi","इंस्टॉल..."},
                {"de","Installieren..."}
            }
        },
        {
            "Offline",
            new Dictionary<string, string>() {
                {"hi","ऑफलाइन"},
                {"fr","Déconnecté"},
                {"zh-cht","離線"},
                {"zh-chs","离线"},
                {"fi","Offline-tilassa"},
                {"tr","Çevrimdışı"},
                {"ja","オフライン"},
                {"es","Desconectado"},
                {"pt","desligada"},
                {"pt-br","desligada"},
                {"sv","Off-line"},
                {"ko","오프라인"},
                {"it","Disconnesso"},
                {"ru","Не в сети"}
            }
        },
        {
            "Timeout",
            new Dictionary<string, string>() {
                {"de","Auszeit"},
                {"hi","समय समाप्त"},
                {"zh-cht","超時"},
                {"zh-chs","超时"},
                {"fi","Aikakatkaisu"},
                {"tr","Zaman aşımı"},
                {"cs","Časový limit"},
                {"ja","タイムアウト"},
                {"pt","Tempo esgotado"},
                {"nl","Time-out"},
                {"pt-br","Tempo esgotado"},
                {"sv","Paus"},
                {"ko","타임 아웃"},
                {"it","Tempo scaduto"},
                {"ru","Тайм-аут"}
            }
        },
        {
            "Compressed Network Traffic",
            new Dictionary<string, string>() {
                {"nl","Gecomprimeerd netwerkverkeer"},
                {"ko","압축 된 네트워크 트래픽"},
                {"fr","Trafic réseau compressé"},
                {"it","Traffico di rete compresso"},
                {"zh-chs","压缩网络流量"},
                {"pt-br","Tráfego de rede comprimido"},
                {"sv","Komprimerad nätverkstrafik"},
                {"es","Tráfico de red comprimido"},
                {"hi","संपीडित नेटवर्क यातायात"},
                {"de","Komprimierter Netzwerkverkehr"}
            }
        },
        {
            "Disconnect",
            new Dictionary<string, string>() {
                {"de","Trennen"},
                {"hi","डिस्कनेक्ट"},
                {"fr","Déconnecter"},
                {"zh-cht","斷線"},
                {"zh-chs","断线"},
                {"fi","Katkaise yhteys"},
                {"tr","Bağlantıyı kes"},
                {"cs","Odpojit"},
                {"ja","切断する"},
                {"es","Desconectar"},
                {"pt","Desconectar"},
                {"nl","Verbreken"},
                {"pt-br","desconectar"},
                {"sv","Koppla ifrån"},
                {"ko","연결 해제"},
                {"it","Disconnetti"},
                {"ru","Разъединить"}
            }
        },
        {
            "Remote Desktop Settings",
            new Dictionary<string, string>() {
                {"de","Einstellungen des entfernten Desktops"},
                {"hi","दूरस्थ डेस्कटॉप सेटिंग्स"},
                {"fr","Paramètres du bureau à distance"},
                {"zh-cht","遠程桌面設置"},
                {"zh-chs","远程桌面设置"},
                {"fi","Etätyöpöydän asetukset"},
                {"tr","Uzak Masaüstü Ayarları"},
                {"cs","Nastavení vzdálené plochy"},
                {"ja","リモートデスクトップ設定"},
                {"es","Opciones de Escritorio Remoto"},
                {"pt","Configurações da área de trabalho remota"},
                {"nl","Instellingen extern bureaublad"},
                {"pt-br","Configurações de área de trabalho remota"},
                {"sv","Fjärrskrivbordsinställningar"},
                {"ko","원격 데스크톱 설정"},
                {"it","Impostazioni desktop remoto"},
                {"ru","Настройки удаленного рабочего стола"}
            }
        },
        {
            "Installation",
            new Dictionary<string, string>() {
                {"nl","Installatie"},
                {"ko","설치"},
                {"it","Installazione"},
                {"zh-chs","安装"},
                {"pt-br","Instalação"},
                {"es","Instalación"},
                {"hi","इंस्टालेशन"}
            }
        },
        {
            "Remote Port",
            new Dictionary<string, string>() {
                {"nl","Externe poort"},
                {"ko","원격 포트"},
                {"fr","Port distant"},
                {"it","Porta remota"},
                {"zh-chs","远程端口"},
                {"pt-br","Porta Remota"},
                {"sv","Fjärrport"},
                {"es","Puerto Remoto"},
                {"hi","रिमोट पोर्ट"},
                {"de","Remote-Port"}
            }
        },
        {
            "Stopped.",
            new Dictionary<string, string>() {
                {"nl","Gestopt."},
                {"ko","중지되었습니다."},
                {"fr","Arrêté."},
                {"zh-chs","停了。"},
                {"pt-br","Parou."},
                {"sv","Stannade."},
                {"es","Detenido."},
                {"hi","रोका हुआ।"},
                {"de","Gestoppt."}
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

        static public string T(string english, string lang)
        {
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
