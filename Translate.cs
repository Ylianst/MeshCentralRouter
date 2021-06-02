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
            "Email sent",
            new Dictionary<string, string>() {
                {"ko","メールを送信しました"},
                {"fr","Email envoyé"},
                {"es","Email enviado"},
                {"ja","メールを送信しました"},
                {"hi","ईमेल भेजा"},
                {"zh-chs","邮件已发送"},
                {"de","E-Mail gesendet"}
            }
        },
        {
            "R&efresh",
            new Dictionary<string, string>() {
                {"ko","リフレッシュ"},
                {"fr","Rafraîchir"},
                {"es","Actualizar"},
                {"ja","リフレッシュ"},
                {"hi","ताज़ा करें"},
                {"zh-chs","刷新"},
                {"de","Aktualisierung"}
            }
        },
        {
            "Sort by &Name",
            new Dictionary<string, string>() {
                {"ko","名前順"},
                {"fr","Trier par nom"},
                {"es","Ordenar por nombre"},
                {"ja","名前順"},
                {"hi","नाम द्वारा क्रमबद्ध करें"},
                {"zh-chs","按名称分类"},
                {"de","Nach Name sortieren"}
            }
        },
        {
            "Changing language will close this tool. Are you sure?",
            new Dictionary<string, string>() {
                {"ko","言語を変更すると、このツールが閉じます。本気ですか？"},
                {"fr","Le changement de langue fermera cet outil. Êtes-vous sûr?"},
                {"es","El cambio de idioma cerrará esta herramienta. ¿Está seguro?"},
                {"ja","言語を変更すると、このツールが閉じます。本気ですか？"},
                {"hi","भाषा बदलने से यह टूल बंद हो जाएगा। क्या आपको यकीन है?"},
                {"zh-chs","更改语言将关闭此工具。你确定吗？"},
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
                {"ko","장치 그룹"},
                {"ru","Группа устройства"}
            }
        },
        {
            "Remote desktop quality, scaling and frame rate settings. These can be adjusted depending on the quality of the network connection.",
            new Dictionary<string, string>() {
                {"ko","リモート デスクトップの品質、スケーリング、フレーム レートの設定。これらは、ネットワーク接続の品質に応じて調整できます。"},
                {"fr","Paramètres de qualité, de mise à l'échelle et de fréquence d'images du bureau à distance. Ceux-ci peuvent être ajustés en fonction de la qualité de la connexion réseau."},
                {"es","Configuración de la calidad del escritorio remoto, la escala y la velocidad de fotogramas. Estos se pueden ajustar en función de la calidad de la conexión de red."},
                {"ja","リモート デスクトップの品質、スケーリング、フレーム レートの設定。これらは、ネットワーク接続の品質に応じて調整できます。"},
                {"hi","दूरस्थ डेस्कटॉप गुणवत्ता, स्केलिंग और फ्रेम दर सेटिंग्स। इन्हें नेटवर्क कनेक्शन की गुणवत्ता के आधार पर समायोजित किया जा सकता है।"},
                {"zh-chs","远程桌面质量、缩放和帧速率设置。这些可以根据网络连接的质量进行调整。"},
                {"de","Remote-Desktop-Qualität, Skalierung und Bildrateneinstellungen. Diese können je nach Qualität der Netzwerkverbindung angepasst werden."}
            }
        },
        {
            "label1",
            new Dictionary<string, string>() {
                {"ko","ラベル1"},
                {"fr","étiquette1"},
                {"es","etiqueta1"},
                {"ja","ラベル1"},
                {"hi","लेबल1"},
                {"zh-chs","标签 1"},
                {"de","Etikett1"}
            }
        },
        {
            "Application",
            new Dictionary<string, string>() {
                {"ko","応用"},
                {"es","Solicitud"},
                {"ja","応用"},
                {"hi","आवेदन"},
                {"zh-chs","应用"},
                {"de","Anwendung"}
            }
        },
        {
            "Remove {0} items?",
            new Dictionary<string, string>() {
                {"ko","{0} 個のアイテムを削除しますか?"},
                {"fr","Supprimer {0} éléments ?"},
                {"es","¿Eliminar {0} elementos?"},
                {"ja","{0} 個のアイテムを削除しますか?"},
                {"hi","{0} आइटम निकालें?"},
                {"zh-chs","删除 {0} 项？"},
                {"de","{0} Elemente entfernen?"}
            }
        },
        {
            "Forward all keyboard keys",
            new Dictionary<string, string>() {
                {"ko","すべてのキーボード キーを転送する"},
                {"fr","Transférer toutes les touches du clavier"},
                {"es","Reenviar todas las teclas del teclado"},
                {"ja","すべてのキーボード キーを転送する"},
                {"hi","सभी कीबोर्ड कुंजियों को अग्रेषित करें"},
                {"zh-chs","转发所有键盘键"},
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
                {"ko","에이전트"},
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
                {"ko","연결 요청"},
                {"ru","Спросите согласия"}
            }
        },
        {
            "Add &Map...",
            new Dictionary<string, string>() {
                {"ko","マップを追加..."},
                {"fr","Ajouter une &carte..."},
                {"es","Agregar & mapa ..."},
                {"ja","マップを追加..."},
                {"hi","नक्शा जोड़ें..."},
                {"zh-chs","添加地图 (&M)..."},
                {"de","&Zuordnen hinzufügen..."}
            }
        },
        {
            "Stopped",
            new Dictionary<string, string>() {
                {"ko","停止"},
                {"fr","Arrêté"},
                {"es","Detenido"},
                {"ja","停止"},
                {"hi","रोका हुआ"},
                {"zh-chs","停止"},
                {"de","Gestoppt"}
            }
        },
        {
            "Two-factor Authentication",
            new Dictionary<string, string>() {
                {"ko","二要素認証"},
                {"fr","Authentification à deux facteurs"},
                {"es","Autenticación de dos factores"},
                {"ja","二要素認証"},
                {"hi","दो तरीकों से प्रमाणीकरण"},
                {"zh-chs","两因素身份验证"},
                {"de","Zwei-Faktor-Authentifizierung"}
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
                {"ko","품질"},
                {"ru","Качество"}
            }
        },
        {
            "0 Bytes",
            new Dictionary<string, string>() {
                {"ko","0 バイト"},
                {"fr","0 octet"},
                {"es","0 bytes"},
                {"ja","0 バイト"},
                {"hi","0 बाइट्स"},
                {"zh-chs","0 字节"},
                {"de","0 Byte"}
            }
        },
        {
            "Routing Stats",
            new Dictionary<string, string>() {
                {"ko","ルーティング統計"},
                {"fr","Statistiques de routage"},
                {"es","Estadísticas de enrutamiento"},
                {"ja","ルーティング統計"},
                {"hi","रूटिंग आँकड़े"},
                {"zh-chs","路由统计"},
                {"de","Routing-Statistiken"}
            }
        },
        {
            ", Recorded Session",
            new Dictionary<string, string>() {
                {"ko","、記録されたセッション"},
                {"fr",", Séance enregistrée"},
                {"es",", Sesión grabada"},
                {"ja","、記録されたセッション"},
                {"hi",", रिकॉर्ड किया गया सत्र"},
                {"zh-chs",", 录制会话"},
                {"de",", Aufgezeichnete Sitzung"}
            }
        },
        {
            "No Port Mappings\r\n\r\nClick \"Add\" to get started.",
            new Dictionary<string, string>() {
                {"ko","ポート マッピングなし\r\n\r\n[追加] をクリックして開始します。"},
                {"fr","Aucun mappage de port\r\n\r\nCliquez sur \"Ajouter\" pour commencer."},
                {"es","Sin asignaciones de puertos\r\n\r\nHaga clic en \"Agregar\" para comenzar."},
                {"ja","ポート マッピングなし\r\n\r\n[追加] をクリックして開始します。"},
                {"hi","कोई पोर्ट मैपिंग नहीं\r\n\r\nआरंभ करने के लिए \"जोड़ें\" पर क्लिक करें।"},
                {"zh-chs","无端口映射\r\n\r\n单击“添加”开始。"},
                {"de","Keine Portzuordnungen\r\n\r\nKlicken Sie auf \"Hinzufügen\", um zu beginnen."}
            }
        },
        {
            "Email verification required",
            new Dictionary<string, string>() {
                {"ko","メール認証が必要です"},
                {"fr","Vérification de l'e-mail requise"},
                {"es","Se requiere verificación de correo electrónico"},
                {"ja","メール認証が必要です"},
                {"hi","ईमेल सत्यापन आवश्यक"},
                {"zh-chs","需要电子邮件验证"},
                {"de","E-Mail-Verifizierung erforderlich"}
            }
        },
        {
            "Relay Mapping",
            new Dictionary<string, string>() {
                {"ko","リレーマッピング"},
                {"fr","Cartographie des relais"},
                {"es","Mapeo de relés"},
                {"ja","リレーマッピング"},
                {"hi","रिले मैपिंग"},
                {"zh-chs","中继映射"},
                {"de","Relaiszuordnung"}
            }
        },
        {
            "Remote Port",
            new Dictionary<string, string>() {
                {"ko","リモートポート"},
                {"fr","Port distant"},
                {"es","Puerto remoto"},
                {"ja","リモートポート"},
                {"hi","रिमोट पोर्ट"},
                {"zh-chs","远程端口"},
                {"de","Remote-Port"}
            }
        },
        {
            "SSH Username",
            new Dictionary<string, string>() {
                {"ko","SSH ユーザー名"},
                {"fr","Nom d'utilisateur SSH"},
                {"es","Nombre de usuario SSH"},
                {"ja","SSH ユーザー名"},
                {"hi","एसएसएच उपयोगकर्ता नाम"},
                {"zh-chs","SSH 用户名"},
                {"de","SSH-Benutzername"}
            }
        },
        {
            ", {0} connections.",
            new Dictionary<string, string>() {
                {"ko","、{0} 接続。"},
                {"fr",", {0} connexions."},
                {"es",", {0} conexiones."},
                {"ja","、{0} 接続。"},
                {"hi",", {0} कनेक्शन।"},
                {"zh-chs",", {0} 个连接。"},
                {"de",", {0} Verbindungen."}
            }
        },
        {
            "Confirm Delete",
            new Dictionary<string, string>() {
                {"ko","削除を確認"},
                {"fr","Confirmation de la suppression"},
                {"es","Confirmar eliminación"},
                {"ja","削除を確認"},
                {"hi","हटाने की पुष्टि करें"},
                {"zh-chs","确认删除"},
                {"de","Löschen bestätigen"}
            }
        },
        {
            "Updating...",
            new Dictionary<string, string>() {
                {"ko","更新中..."},
                {"fr","Mise à jour..."},
                {"es","Actualizando ..."},
                {"ja","更新中..."},
                {"hi","अपडेट हो रहा है..."},
                {"zh-chs","正在更新..."},
                {"de","Aktualisierung..."}
            }
        },
        {
            "Compressed Network Traffic",
            new Dictionary<string, string>() {
                {"ko","圧縮されたネットワーク トラフィック"},
                {"fr","Trafic réseau compressé"},
                {"es","Tráfico de red comprimido"},
                {"ja","圧縮されたネットワーク トラフィック"},
                {"hi","संपीडित नेटवर्क यातायात"},
                {"zh-chs","压缩网络流量"},
                {"de","Komprimierter Netzwerkverkehr"}
            }
        },
        {
            "&Info...",
            new Dictionary<string, string>() {
                {"ko","&情報..."},
                {"es","&Info ..."},
                {"ja","&情報..."},
                {"hi","जानकारी..."},
                {"zh-chs","＆信息..."},
                {"de","&Die Info..."}
            }
        },
        {
            ", 1 connection.",
            new Dictionary<string, string>() {
                {"ko","、1接続。"},
                {"fr",", 1 connexion."},
                {"es",", 1 conexión."},
                {"ja","、1接続。"},
                {"hi",", 1 कनेक्शन।"},
                {"zh-chs",", 1 个连接。"},
                {"de",", 1 Anschluss."}
            }
        },
        {
            "Rename File",
            new Dictionary<string, string>() {
                {"ko","ファイルの名前を変更"},
                {"fr","Renommer le fichier"},
                {"es","Renombrar archivo"},
                {"ja","ファイルの名前を変更"},
                {"hi","फाइल का नाम बदलो"},
                {"zh-chs","重新命名文件"},
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
                {"ko","이메일"}
            }
        },
        {
            "ComputerName",
            new Dictionary<string, string>() {
                {"ko","コンピュータネーム"},
                {"fr","Nom de l'ordinateur"},
                {"es","Nombre del computador"},
                {"ja","コンピュータネーム"},
                {"hi","कंप्यूटर का नाम"},
                {"zh-chs","计算机名"},
                {"de","Computername"}
            }
        },
        {
            "ServerName",
            new Dictionary<string, string>() {
                {"ko","サーバー名"},
                {"fr","Nom du serveur"},
                {"es","Nombre del servidor"},
                {"ja","サーバー名"},
                {"hi","सर्वर का नाम"},
                {"zh-chs","服务器名称"},
                {"de","Servername"}
            }
        },
        {
            "&Open...",
            new Dictionary<string, string>() {
                {"ko","＆開いた..."},
                {"fr","&Ouvert..."},
                {"es","&Abierto..."},
                {"ja","＆開いた..."},
                {"hi","&खुला हुआ..."},
                {"zh-chs","＆打开..."},
                {"de","&Öffnen..."}
            }
        },
        {
            "Port {0} to port {1}",
            new Dictionary<string, string>() {
                {"ko","ポート {0} からポート {1}"},
                {"fr","Port {0} vers port {1}"},
                {"es","Puerto {0} al puerto {1}"},
                {"ja","ポート {0} からポート {1}"},
                {"hi","पोर्ट {0} से पोर्ट {1}"},
                {"zh-chs","端口 {0} 到端口 {1}"},
                {"de","Port {0} zu Port {1}"}
            }
        },
        {
            "RDP Port",
            new Dictionary<string, string>() {
                {"ko","RDP ポート"},
                {"fr","Port RDP"},
                {"es","Puerto RDP"},
                {"ja","RDP ポート"},
                {"hi","आरडीपी पोर्ट"},
                {"zh-chs","RDP 端口"},
                {"de","RDP-Port"}
            }
        },
        {
            "Enter the RDP port of the remote computer, the default RDP port is 3389.",
            new Dictionary<string, string>() {
                {"ko","リモート コンピューターの RDP ポートを入力します。デフォルトの RDP ポートは 3389 です。"},
                {"fr","Entrez le port RDP de l'ordinateur distant, le port RDP par défaut est 3389."},
                {"es","Ingrese el puerto RDP de la computadora remota, el puerto RDP predeterminado es 3389."},
                {"ja","リモート コンピューターの RDP ポートを入力します。デフォルトの RDP ポートは 3389 です。"},
                {"hi","दूरस्थ कंप्यूटर का RDP पोर्ट दर्ज करें, डिफ़ॉल्ट RDP पोर्ट 3389 है।"},
                {"zh-chs","输入远程计算机的RDP端口，默认RDP端口为3389。"},
                {"de","Geben Sie den RDP-Port des Remote-Computers ein, der Standard-RDP-Port ist 3389."}
            }
        },
        {
            "Certificate",
            new Dictionary<string, string>() {
                {"ko","証明書"},
                {"fr","Certificat"},
                {"es","Certificado"},
                {"ja","証明書"},
                {"hi","प्रमाणपत्र"},
                {"zh-chs","证书"},
                {"de","Zertifikat"}
            }
        },
        {
            "Set RDP port...",
            new Dictionary<string, string>() {
                {"ko","RDP ポートを設定..."},
                {"fr","Définir le port RDP..."},
                {"es","Establecer puerto RDP ..."},
                {"ja","RDP ポートを設定..."},
                {"hi","आरडीपी पोर्ट सेट करें..."},
                {"zh-chs","设置 RDP 端口..."},
                {"de","RDP-Port einstellen..."}
            }
        },
        {
            "Server information",
            new Dictionary<string, string>() {
                {"ko","サーバー情報"},
                {"fr","Informations sur le serveur"},
                {"es","Información del servidor"},
                {"ja","サーバー情報"},
                {"hi","सर्वर जानकारी"},
                {"zh-chs","服务器信息"},
                {"de","Serverinformation"}
            }
        },
        {
            "Incoming Compression",
            new Dictionary<string, string>() {
                {"ko","着信圧縮"},
                {"fr","Compression entrante"},
                {"es","Compresión entrante"},
                {"ja","着信圧縮"},
                {"hi","आने वाली संपीड़न"},
                {"zh-chs","传入压缩"},
                {"de","Eingehende Kompression"}
            }
        },
        {
            "Failed to start remote desktop session",
            new Dictionary<string, string>() {
                {"ko","リモート デスクトップ セッションを開始できませんでした"},
                {"fr","Échec du démarrage de la session de bureau à distance"},
                {"es","No se pudo iniciar la sesión de escritorio remoto"},
                {"ja","リモート デスクトップ セッションを開始できませんでした"},
                {"hi","दूरस्थ डेस्कटॉप सत्र प्रारंभ करने में विफल"},
                {"zh-chs","无法启动远程桌面会话"},
                {"de","Fehler beim Starten der Remote-Desktop-Sitzung"}
            }
        },
        {
            "Transfer Progress",
            new Dictionary<string, string>() {
                {"ko","転送の進行状況"},
                {"fr","Progression du transfert"},
                {"es","Progreso de la transferencia"},
                {"ja","転送の進行状況"},
                {"hi","स्थानांतरण प्रगति"},
                {"zh-chs","转学进度"},
                {"de","Übertragungsfortschritt"}
            }
        },
        {
            "Show on system tray",
            new Dictionary<string, string>() {
                {"ko","システムトレイに表示"},
                {"fr","Afficher sur la barre d'état système"},
                {"es","Mostrar en la bandeja del sistema"},
                {"ja","システムトレイに表示"},
                {"hi","सिस्टम ट्रे पर दिखाएं"},
                {"zh-chs","在系统托盘上显示"},
                {"de","In der Taskleiste anzeigen"}
            }
        },
        {
            "E&xit",
            new Dictionary<string, string>() {
                {"ko","出口"},
                {"fr","Sortir"},
                {"es","Salida"},
                {"ja","出口"},
                {"hi","बाहर जाएं"},
                {"zh-chs","出口"},
                {"de","Ausgang"}
            }
        },
        {
            "Remote - {0}",
            new Dictionary<string, string>() {
                {"ko","リモート - {0}"},
                {"fr","À distance - {0}"},
                {"es","Remoto: {0}"},
                {"ja","リモート - {0}"},
                {"hi","रिमोट - {0}"},
                {"zh-chs","远程 - {0}"},
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
                {"ko","크기"},
                {"ru","Размер"}
            }
        },
        {
            "Don't ask for {0} days.",
            new Dictionary<string, string>() {
                {"ko","{0} 日間尋ねないでください。"},
                {"fr","Ne demandez pas {0} jours."},
                {"es","No pida {0} días."},
                {"ja","{0} 日間尋ねないでください。"},
                {"hi","{0} दिनों के लिए मत पूछो।"},
                {"zh-chs","不要要求 {0} 天。"},
                {"de","Frag nicht nach {0} Tagen."}
            }
        },
        {
            "Site",
            new Dictionary<string, string>() {
                {"ko","地点"},
                {"fr","Placer"},
                {"es","Sitio"},
                {"ja","地点"},
                {"hi","साइट"},
                {"zh-chs","地点"},
                {"de","Seite? ˅"}
            }
        },
        {
            "Failed to start remote terminal session",
            new Dictionary<string, string>() {
                {"ko","リモート ターミナル セッションの開始に失敗しました"},
                {"fr","Échec du démarrage de la session de terminal distant"},
                {"es","No se pudo iniciar la sesión de terminal remota"},
                {"ja","リモート ターミナル セッションの開始に失敗しました"},
                {"hi","दूरस्थ टर्मिनल सत्र प्रारंभ करने में विफल"},
                {"zh-chs","无法启动远程终端会话"},
                {"de","Fehler beim Starten der Remote-Terminal-Sitzung"}
            }
        },
        {
            "Connection",
            new Dictionary<string, string>() {
                {"ko","接続"},
                {"fr","Lien"},
                {"es","Conexión"},
                {"ja","接続"},
                {"hi","संबंध"},
                {"zh-chs","联系"},
                {"de","Verbindung"}
            }
        },
        {
            "Remote IP",
            new Dictionary<string, string>() {
                {"ko","リモート IP"},
                {"fr","IP distante"},
                {"es","IP remota"},
                {"ja","リモート IP"},
                {"hi","दूरदराज़ के आई. पी"},
                {"zh-chs","远程IP"},
                {"de","Remote-IP"}
            }
        },
        {
            "HTTP",
            new Dictionary<string, string>() {
                {"hi","एचटीटीपी"}
            }
        },
        {
            "Stats...",
            new Dictionary<string, string>() {
                {"ko","統計..."},
                {"fr","Statistiques..."},
                {"es","Estadísticas ..."},
                {"ja","統計..."},
                {"hi","आँकड़े..."},
                {"zh-chs","统计..."},
                {"de","Statistiken..."}
            }
        },
        {
            "Local Port",
            new Dictionary<string, string>() {
                {"ko","ローカルポート"},
                {"fr","Port local"},
                {"es","Puerto local"},
                {"ja","ローカルポート"},
                {"hi","स्थानीय बंदरगाह"},
                {"zh-chs","本地端口"},
                {"de","Lokaler Hafen"}
            }
        },
        {
            "MeshCentral",
            new Dictionary<string, string>() {
                {"ko","メッシュ中央"},
                {"ja","メッシュ中央"},
                {"hi","मेशसेंट्रल"},
                {"zh-chs","网格中心"}
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
                {"ko","사용자가 액세스 권한을 부여하기를 기다리는 중 ..."},
                {"ru","Ожидание предоставления доступа пользователем ..."}
            }
        },
        {
            "Remote Desktop...",
            new Dictionary<string, string>() {
                {"ko","リモートデスクトップ..."},
                {"fr","Bureau à distance..."},
                {"es","Escritorio remoto..."},
                {"ja","リモートデスクトップ..."},
                {"hi","रिमोट डेस्कटॉप..."},
                {"zh-chs","远程桌面..."},
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
                {"ko","원격 데스크탑"},
                {"ru","Удаленного рабочего стола"}
            }
        },
        {
            "Toggle remote desktop connection",
            new Dictionary<string, string>() {
                {"ko","リモート デスクトップ接続を切り替える"},
                {"fr","Basculer la connexion au bureau à distance"},
                {"es","Alternar la conexión de escritorio remoto"},
                {"ja","リモート デスクトップ接続を切り替える"},
                {"hi","दूरस्थ डेस्कटॉप कनेक्शन टॉगल करें"},
                {"zh-chs","切换远程桌面连接"},
                {"de","Remotedesktopverbindung umschalten"}
            }
        },
        {
            "Log out",
            new Dictionary<string, string>() {
                {"ko","ログアウト"},
                {"fr","Se déconnecter"},
                {"es","Cerrar sesión"},
                {"ja","ログアウト"},
                {"hi","लॉग आउट"},
                {"zh-chs","登出"},
                {"de","Ausloggen"}
            }
        },
        {
            "Pull clipboard from remote device",
            new Dictionary<string, string>() {
                {"ko","リモート デバイスからクリップボードをプルする"},
                {"fr","Extraire le presse-papiers de l'appareil distant"},
                {"es","Extraiga el portapapeles del dispositivo remoto"},
                {"ja","リモート デバイスからクリップボードをプルする"},
                {"hi","दूरस्थ डिवाइस से क्लिपबोर्ड खींचे"},
                {"zh-chs","从远程设备拉剪贴板"},
                {"de","Zwischenablage von Remote-Gerät ziehen"}
            }
        },
        {
            "View Certificate Details...",
            new Dictionary<string, string>() {
                {"ko","証明書の詳細を表示..."},
                {"fr","Afficher les détails du certificat..."},
                {"es","Ver detalles del certificado ..."},
                {"ja","証明書の詳細を表示..."},
                {"hi","प्रमाणपत्र विवरण देखें..."},
                {"zh-chs","查看证书详细信息..."},
                {"de","Zertifikatdetails anzeigen..."}
            }
        },
        {
            "Invalid username or password",
            new Dictionary<string, string>() {
                {"ko","無効なユーザー名またはパスワード"},
                {"fr","Nom d'utilisateur ou mot de passe invalide"},
                {"es","usuario o contraseña invalido"},
                {"ja","無効なユーザー名またはパスワード"},
                {"hi","अमान्य उपयोगकर्ता नाम या पासवर्ड"},
                {"zh-chs","无效的用户名或密码"},
                {"de","ungültiger Benutzername oder Passwort"}
            }
        },
        {
            "&Rename",
            new Dictionary<string, string>() {
                {"ko","名前を変更"},
                {"fr","&Renommer"},
                {"es","&Rebautizar"},
                {"ja","名前を変更"},
                {"hi","&नाम बदलें"},
                {"zh-chs","＆改名"},
                {"de","&Umbenennen"}
            }
        },
        {
            "Application Link",
            new Dictionary<string, string>() {
                {"ko","アプリケーションリンク"},
                {"fr","Lien d'application"},
                {"es","Enlace de aplicación"},
                {"ja","アプリケーションリンク"},
                {"hi","आवेदन लिंक"},
                {"zh-chs","申请链接"},
                {"de","Bewerbungslink"}
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
                {"ko","연결 요청 + Bar"},
                {"ru","Спросите согласия + бар"}
            }
        },
        {
            "{0} Bytes",
            new Dictionary<string, string>() {
                {"ko","{0} バイト"},
                {"fr","{0} octets"},
                {"es","{0} bytes"},
                {"ja","{0} バイト"},
                {"hi","{0} बाइट्स"},
                {"zh-chs","{0} 字节"},
                {"de","{0} Byte"}
            }
        },
        {
            "Outgoing Bytes",
            new Dictionary<string, string>() {
                {"ko","送信バイト"},
                {"fr","Octets sortants"},
                {"es","Bytes salientes"},
                {"ja","送信バイト"},
                {"hi","आउटगोइंग बाइट्स"},
                {"zh-chs","传出字节"},
                {"de","Ausgehende Bytes"}
            }
        },
        {
            "Show &Group Names",
            new Dictionary<string, string>() {
                {"ko","グループ名を表示"},
                {"fr","Afficher les noms de &groupes"},
                {"es","Mostrar y nombres de grupos"},
                {"ja","グループ名を表示"},
                {"hi","समूह के नाम दिखाएं"},
                {"zh-chs","显示组名称(&G)"},
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
                {"es","Nombre de usuario"},
                {"pt","Nome de usuário"},
                {"nl","Gebruikersnaam"},
                {"ko","사용자 이름"},
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
                {"ko","연결 해제"},
                {"ru","Отключен"}
            }
        },
        {
            "No Devices",
            new Dictionary<string, string>() {
                {"ko","デバイスなし"},
                {"fr","Aucun appareil"},
                {"es","Sin dispositivos"},
                {"ja","デバイスなし"},
                {"hi","कोई उपकरण नहीं"},
                {"zh-chs","没有设备"},
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
                {"ko","상태"},
                {"ru","Состояние"}
            }
        },
        {
            "Add &Relay Map...",
            new Dictionary<string, string>() {
                {"ko","リレー マップを追加..."},
                {"fr","Ajouter une carte de &relais..."},
                {"es","Agregar y retransmitir mapa ..."},
                {"ja","リレー マップを追加..."},
                {"hi","मानचित्र &रिले जोड़ें..."},
                {"zh-chs","添加中继地图 (&R)..."},
                {"de","&Relay-Karte hinzufügen..."}
            }
        },
        {
            "Add Map...",
            new Dictionary<string, string>() {
                {"ko","地図を追加..."},
                {"fr","Ajouter une carte..."},
                {"es","Agregar mapa ..."},
                {"ja","地図を追加..."},
                {"hi","नक्शा जोड़ें..."},
                {"zh-chs","添加地图..."},
                {"de","Karte hinzufügen..."}
            }
        },
        {
            "SMS sent",
            new Dictionary<string, string>() {
                {"ko","SMSが送信されました"},
                {"fr","SMS envoyé"},
                {"es","SMS enviado"},
                {"ja","SMSが送信されました"},
                {"hi","एसएमएस भेजा गया"},
                {"zh-chs","短信发送"},
                {"de","SMS gesendet"}
            }
        },
        {
            "&Open Mappings...",
            new Dictionary<string, string>() {
                {"ko","マッピングを開く..."},
                {"fr","&Ouvrir les mappages..."},
                {"es","&Abrir mapeos ..."},
                {"ja","マッピングを開く..."},
                {"hi","&ओपन मैपिंग..."},
                {"zh-chs","打开映射 (&O)..."},
                {"de","&Zuordnungen öffnen..."}
            }
        },
        {
            "PuTTY SSH client",
            new Dictionary<string, string>() {
                {"ko","PuTTY SSH クライアント"},
                {"fr","Client SSH PuTTY"},
                {"es","Cliente PuTTY SSH"},
                {"ja","PuTTY SSH クライアント"},
                {"hi","पुटी एसएसएच क्लाइंट"},
                {"zh-chs","PuTTY SSH 客户端"},
                {"de","PuTTY SSH-Client"}
            }
        },
        {
            "Unable to connect",
            new Dictionary<string, string>() {
                {"ko","接続することができません"},
                {"fr","Impossible de se connecter"},
                {"es","No puede conectarse"},
                {"ja","接続することができません"},
                {"hi","कनेक्ट करने में असमर्थ"},
                {"zh-chs","无法连接"},
                {"de","Verbindung konnte nicht hergestellt werden"}
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
                {"ko","파일 작업"},
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
                {"ko","취소"},
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
                {"ko","연결됨"},
                {"ru","Подключено"}
            }
        },
        {
            "Display {0}",
            new Dictionary<string, string>() {
                {"nl","Scherm {0}"},
                {"ko","{0}を表示"},
                {"fr","Affichage {0}"},
                {"zh-chs","显示{0}"},
                {"es","Mostrar {0}"},
                {"ja","{0}を表示"},
                {"ru","Экран {0}"},
                {"hi","प्रदर्शन {0}"},
                {"de","Anzeige {0}"}
            }
        },
        {
            "Server",
            new Dictionary<string, string>() {
                {"ko","サーバ"},
                {"fr","Serveur"},
                {"es","Servidor"},
                {"ja","サーバ"},
                {"hi","सर्वर"},
                {"zh-chs","服务器"}
            }
        },
        {
            "Install...",
            new Dictionary<string, string>() {
                {"ko","インストール..."},
                {"fr","Installer..."},
                {"es","Instalar en pc..."},
                {"ja","インストール..."},
                {"hi","इंस्टॉल..."},
                {"zh-chs","安装..."},
                {"de","Installieren..."}
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
            "Device Settings",
            new Dictionary<string, string>() {
                {"ko","デバイスの設定"},
                {"fr","Réglages de l'appareil"},
                {"es","Configuración de dispositivo"},
                {"ja","デバイスの設定"},
                {"hi","उपकरण सेटिंग्स"},
                {"zh-chs","设备设置"},
                {"de","Geräteeinstellungen"}
            }
        },
        {
            "Port Mapping",
            new Dictionary<string, string>() {
                {"ko","ポートマッピング"},
                {"fr","Mappage des ports"},
                {"es","La asignación de puertos"},
                {"ja","ポートマッピング"},
                {"hi","पोर्ट मानचित्रण"},
                {"zh-chs","端口映射"},
                {"de","Port-Mapping"}
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
                {"ko","MeshCentral 라우터"},
                {"ru","MeshCentral Router "}
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
                {"ko","토큰"},
                {"ru","Токен"}
            }
        },
        {
            "Add Relay Map...",
            new Dictionary<string, string>() {
                {"ko","リレー マップを追加..."},
                {"fr","Ajouter une carte de relais..."},
                {"es","Agregar mapa de retransmisiones ..."},
                {"ja","リレー マップを追加..."},
                {"hi","रिले मैप जोड़ें..."},
                {"zh-chs","添加中继地图..."},
                {"de","Relaiskarte hinzufügen..."}
            }
        },
        {
            "Recursive Delete",
            new Dictionary<string, string>() {
                {"ko","再帰的削除"},
                {"fr","Suppression récursive"},
                {"es","Eliminación recursiva"},
                {"ja","再帰的削除"},
                {"hi","पुनरावर्ती हटाएं"},
                {"zh-chs","递归删除"},
                {"de","Rekursives Löschen"}
            }
        },
        {
            "&Delete",
            new Dictionary<string, string>() {
                {"ko","削除"},
                {"fr","&Effacer"},
                {"es","&Borrar"},
                {"ja","削除"},
                {"hi","&हटाएं"},
                {"zh-chs","＆删除"},
                {"de","&Löschen"}
            }
        },
        {
            "Starting...",
            new Dictionary<string, string>() {
                {"ko","起動..."},
                {"fr","Départ..."},
                {"es","A partir de..."},
                {"ja","起動..."},
                {"hi","शुरुआत..."},
                {"zh-chs","开始..."},
                {"de","Beginnend..."}
            }
        },
        {
            "&Save Mappings...",
            new Dictionary<string, string>() {
                {"ko","マッピングを保存..."},
                {"fr","&Enregistrer les mappages..."},
                {"es","&Guardar asignaciones ..."},
                {"ja","マッピングを保存..."},
                {"hi","&मैपिंग सहेजें..."},
                {"zh-chs","保存映射(&S)..."},
                {"de","&Zuordnungen speichern..."}
            }
        },
        {
            "Toggle zoom-to-fit mode",
            new Dictionary<string, string>() {
                {"ko","ズーム ツー フィット モードを切り替える"},
                {"fr","Basculer en mode zoom pour ajuster"},
                {"es","Alternar el modo de zoom para ajustar"},
                {"ja","ズーム ツー フィット モードを切り替える"},
                {"hi","ज़ूम-टू-फ़िट मोड टॉगल करें"},
                {"zh-chs","切换缩放至适合模式"},
                {"de","Zoom-to-Fit-Modus umschalten"}
            }
        },
        {
            "Relay Device",
            new Dictionary<string, string>() {
                {"ko","中継装置"},
                {"fr","Dispositif de relais"},
                {"es","Dispositivo de retransmisión"},
                {"ja","中継装置"},
                {"hi","रिले डिवाइस"},
                {"zh-chs","中继装置"},
                {"de","Relaisgerät"}
            }
        },
        {
            "Mapping Settings",
            new Dictionary<string, string>() {
                {"ko","マッピング設定"},
                {"fr","Paramètres de mappage"},
                {"es","Configuración de mapeo"},
                {"ja","マッピング設定"},
                {"hi","मैपिंग सेटिंग्स"},
                {"zh-chs","映射设置"},
                {"de","Zuordnungseinstellungen"}
            }
        },
        {
            "Port {0} to {1}:{2}",
            new Dictionary<string, string>() {
                {"ko","ポート {0} から {1}:{2}"},
                {"fr","Port {0} vers {1} :{2}"},
                {"es","Puerto {0} a {1}: {2}"},
                {"ja","ポート {0} から {1}:{2}"},
                {"hi","पोर्ट {0} से {1}:{2}"},
                {"zh-chs","端口 {0} 到 {1}：{2}"},
                {"de","Port {0} nach {1}:{2}"}
            }
        },
        {
            "Create Folder",
            new Dictionary<string, string>() {
                {"ko","フォルダーを作る"},
                {"fr","Créer le dossier"},
                {"es","Crear carpeta"},
                {"ja","フォルダーを作る"},
                {"hi","फोल्डर बनाएं"},
                {"zh-chs","创建文件夹"},
                {"de","Ordner erstellen"}
            }
        },
        {
            "Application Launch",
            new Dictionary<string, string>() {
                {"ko","アプリケーションの起動"},
                {"fr","Lancement de l'application"},
                {"es","Lanzamiento de la aplicación"},
                {"ja","アプリケーションの起動"},
                {"hi","एप्लिकेशन लॉन्च"},
                {"zh-chs","应用启动"},
                {"de","Anwendungsstart"}
            }
        },
        {
            "Bind local port to all network interfaces",
            new Dictionary<string, string>() {
                {"ko","ローカル ポートをすべてのネットワーク インターフェイスにバインドする"},
                {"fr","Lier le port local à toutes les interfaces réseau"},
                {"es","Vincular el puerto local a todas las interfaces de red"},
                {"ja","ローカル ポートをすべてのネットワーク インターフェイスにバインドする"},
                {"hi","स्थानीय पोर्ट को सभी नेटवर्क इंटरफेस से बाइंड करें"},
                {"zh-chs","将本地端口绑定到所有网络接口"},
                {"de","Binden Sie den lokalen Port an alle Netzwerkschnittstellen"}
            }
        },
        {
            "Installation",
            new Dictionary<string, string>() {
                {"ko","インストール"},
                {"es","Instalación"},
                {"ja","インストール"},
                {"hi","इंस्टालेशन"},
                {"zh-chs","安装"}
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
                {"ko","빠른"},
                {"ru","Быстро"}
            }
        },
        {
            "Remote Files",
            new Dictionary<string, string>() {
                {"ko","リモート ファイル"},
                {"fr","Fichiers distants"},
                {"es","Archivos remotos"},
                {"ja","リモート ファイル"},
                {"hi","दूरस्थ फ़ाइलें"},
                {"zh-chs","远程文件"},
                {"de","Remote-Dateien"}
            }
        },
        {
            "File Transfer",
            new Dictionary<string, string>() {
                {"nl","Bestandsoverdracht"},
                {"ko","ファイル転送"},
                {"fr","Transfert de fichiers"},
                {"zh-chs","文件传输"},
                {"es","Transferencia de archivos"},
                {"ja","ファイル転送"},
                {"ru","Передача файлов"},
                {"hi","फ़ाइल स्थानांतरण"},
                {"de","Datei Übertragung"}
            }
        },
        {
            "Click ok to register MeshCentral Router on your system as the handler for the \"mcrouter://\" protocol. This will allow the MeshCentral web site to launch this application when needed.",
            new Dictionary<string, string>() {
                {"ko","[OK] をクリックして、システムに MeshCentral Router を「mcrouter://」プロトコルのハンドラーとして登録します。これにより、MeshCentral Web サイトは必要に応じてこのアプリケーションを起動できます。"},
                {"fr","Cliquez sur ok pour enregistrer MeshCentral Router sur votre système en tant que gestionnaire du protocole « mcrouter:// ». Cela permettra au site Web MeshCentral de lancer cette application en cas de besoin."},
                {"es","Haga clic en Aceptar para registrar MeshCentral Router en su sistema como el controlador del protocolo \"mcrouter: //\". Esto permitirá que el sitio web de MeshCentral inicie esta aplicación cuando sea necesario."},
                {"ja","[OK] をクリックして、システムに MeshCentral Router を「mcrouter://」プロトコルのハンドラーとして登録します。これにより、MeshCentral Web サイトは必要に応じてこのアプリケーションを起動できます。"},
                {"hi","MeshCentral राउटर को अपने सिस्टम पर \"mcrouter: //\" प्रोटोकॉल के लिए हैंडलर के रूप में पंजीकृत करने के लिए ओके पर क्लिक करें। यह मेशसेंट्रल वेब साइट को जरूरत पड़ने पर इस एप्लिकेशन को लॉन्च करने की अनुमति देगा।"},
                {"zh-chs","单击确定在您的系统上注册 MeshCentral Router 作为“mcrouter://”协议的处理程序。这将允许 MeshCentral 网站在需要时启动此应用程序。"},
                {"de","Klicken Sie auf OK, um MeshCentral Router auf Ihrem System als Handler für das Protokoll \"mcrouter://\" zu registrieren. Dadurch kann die MeshCentral-Website diese Anwendung bei Bedarf starten."}
            }
        },
        {
            "Next",
            new Dictionary<string, string>() {
                {"ko","次"},
                {"fr","Suivant"},
                {"es","próximo"},
                {"ja","次"},
                {"hi","अगला"},
                {"zh-chs","下一个"},
                {"de","Nächster"}
            }
        },
        {
            "WinSCP client",
            new Dictionary<string, string>() {
                {"ko","WinSCP クライアント"},
                {"fr","Client WinSCP"},
                {"es","Cliente WinSCP"},
                {"ja","WinSCP クライアント"},
                {"hi","विनएससीपी क्लाइंट"},
                {"zh-chs","WinSCP客户端 "},
                {"de","WinSCP-Client"}
            }
        },
        {
            "Outgoing Compression",
            new Dictionary<string, string>() {
                {"ko","発信圧縮"},
                {"fr","Compression sortante"},
                {"es","Compresión saliente"},
                {"ja","発信圧縮"},
                {"hi","आउटगोइंग संपीड़न"},
                {"zh-chs","输出压缩"},
                {"de","Ausgehende Komprimierung"}
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
                {"ko","뒤로"},
                {"ru","Назад"}
            }
        },
        {
            "Show &Offline Devices",
            new Dictionary<string, string>() {
                {"ko","オフライン デバイスを表示"},
                {"fr","Afficher les appareils &hors ligne"},
                {"es","Mostrar y dispositivos sin conexión"},
                {"ja","オフライン デバイスを表示"},
                {"hi","दिखाएँ &ऑफ़लाइन उपकरण"},
                {"zh-chs","显示离线设备 (&A)"},
                {"de","&Offline-Geräte anzeigen"}
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
                {"ko","이름 바꾸기"},
                {"ru","Переименовать"}
            }
        },
        {
            "Tunnelling Data",
            new Dictionary<string, string>() {
                {"ko","トンネリングデータ"},
                {"fr","Données de tunneling"},
                {"es","Datos de tunelización"},
                {"ja","トンネリングデータ"},
                {"hi","टनलिंग डेटा"},
                {"zh-chs","隧道数据"},
                {"de","Tunneling-Daten"}
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
                {"ko","제거"},
                {"ru","Удалить"}
            }
        },
        {
            "No Search Results",
            new Dictionary<string, string>() {
                {"ko","検索結果がありません"},
                {"fr","aucun résultat trouvé"},
                {"es","Sin resultados de búsqueda"},
                {"ja","検索結果がありません"},
                {"hi","खोजने पर कोई परिणाम नहीं मिला"},
                {"zh-chs","没有搜索结果"},
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
                {"ko","닫기"},
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
            "Open...",
            new Dictionary<string, string>() {
                {"ko","開いた..."},
                {"fr","Ouvert..."},
                {"es","Abierto..."},
                {"ja","開いた..."},
                {"hi","खुला हुआ..."},
                {"zh-chs","打开..."},
                {"de","Öffnen..."}
            }
        },
        {
            "OpenSSH",
            new Dictionary<string, string>() {
                {"hi","अधिभारित"},
                {"zh-chs","开放式SSH"}
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
                {"ko","검색"},
                {"ru","Поиск"}
            }
        },
        {
            "Remote Files...",
            new Dictionary<string, string>() {
                {"ko","リモート ファイル..."},
                {"fr","Fichiers distants..."},
                {"es","Archivos remotos ..."},
                {"ja","リモート ファイル..."},
                {"hi","दूरस्थ फ़ाइलें..."},
                {"zh-chs","远程文件..."},
                {"de","Remote-Dateien..."}
            }
        },
        {
            "statusStrip1",
            new Dictionary<string, string>() {
                {"hi","स्थिति पट्टी1"},
                {"zh-chs","状态条1"}
            }
        },
        {
            "Open Web Site",
            new Dictionary<string, string>() {
                {"ko","ウェブサイトを開く"},
                {"fr","Ouvrir le site Web"},
                {"es","Abrir sitio web"},
                {"ja","ウェブサイトを開く"},
                {"hi","वेब साइट खोलें"},
                {"zh-chs","打开网站"},
                {"de","Website öffnen"}
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
                {"ko","중간"},
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
            "Device Status",
            new Dictionary<string, string>() {
                {"ko","デバイスの状態"},
                {"fr","Statut du périphérique"},
                {"es","Estado del dispositivo"},
                {"ja","デバイスの状態"},
                {"hi","उपकरण की स्थिति"},
                {"zh-chs","设备状态"},
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
                {"ko","등록된 휴대폰 번호로 토큰을 보내시겠습니까?"},
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
                {"ko","프레임 속도"},
                {"ru","Частота кадров"}
            }
        },
        {
            "TCP",
            new Dictionary<string, string>() {
                {"hi","टीसीपी"}
            }
        },
        {
            "Path",
            new Dictionary<string, string>() {
                {"ko","道"},
                {"fr","Chemin"},
                {"es","Camino"},
                {"ja","道"},
                {"hi","पथ"},
                {"zh-chs","小路"},
                {"de","Pfad"}
            }
        },
        {
            "Use Remote Keyboard Map",
            new Dictionary<string, string>() {
                {"de","Verwenden Sie die Remote Keyboard Map"},
                {"hi","रिमोट कीबोर्ड मैप का उपयोग करें"},
                {"fr","Utiliser la configuration du clavier distant"},
                {"zh-cht","使用遠程鍵盤映射"},
                {"zh-chs","使用远程键盘映射"},
                {"fi","Käytä kaukonäppäimistökarttaa"},
                {"tr","Uzak Klavye Haritasını Kullan"},
                {"cs","Použijte Mapu vzdálené klávesnice"},
                {"ja","リモートキーボードマップを使用する"},
                {"es","Usar mapa de teclado remoto"},
                {"pt","Use o mapa do teclado remoto"},
                {"nl","Gebruik de externe toetsenbord instelling"},
                {"ko","원격 키보드 맵 사용"},
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
                {"ko","통계"},
                {"ru","Статистика"}
            }
        },
        {
            "Enhanced keyboard capture",
            new Dictionary<string, string>() {
                {"ko","強化されたキーボード キャプチャ"},
                {"fr","Capture de clavier améliorée"},
                {"es","Captura de teclado mejorada"},
                {"ja","強化されたキーボード キャプチャ"},
                {"hi","उन्नत कीबोर्ड कैप्चर"},
                {"zh-chs","增强的键盘捕获"},
                {"de","Verbesserte Tastaturerfassung"}
            }
        },
        {
            "Ignore",
            new Dictionary<string, string>() {
                {"ko","無視する"},
                {"fr","Ignorer"},
                {"es","Ignorar"},
                {"ja","無視する"},
                {"hi","नज़रअंदाज़ करना"},
                {"zh-chs","忽略"},
                {"de","Ignorieren"}
            }
        },
        {
            "UDP",
            new Dictionary<string, string>() {
                {"hi","यूडीपी"}
            }
        },
        {
            "MeshCentral Router Update",
            new Dictionary<string, string>() {
                {"ko","MeshCentral ルーターの更新"},
                {"fr","Mise à jour du routeur MeshCentral"},
                {"es","Actualización del enrutador MeshCentral"},
                {"ja","MeshCentral ルーターの更新"},
                {"hi","मेशसेंट्रल राउटर अपडेट"},
                {"zh-chs","MeshCentral 路由器更新"},
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
                {"ko","거부"},
                {"ru","Отказано"}
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
                {"ko","비율"},
                {"ru","Маштабирование"}
            }
        },
        {
            "Desktop Settings",
            new Dictionary<string, string>() {
                {"ko","デスクトップ設定"},
                {"fr","Paramètres du bureau"},
                {"es","Configuración de escritorio"},
                {"ja","デスクトップ設定"},
                {"hi","डेस्कटॉप सेटिंग्स"},
                {"zh-chs","桌面设置"},
                {"de","Desktop-Einstellungen"}
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
                {"ko","이름"},
                {"ru","Имя"}
            }
        },
        {
            "Remote Desktop Data",
            new Dictionary<string, string>() {
                {"ko","リモート デスクトップ データ"},
                {"fr","Données de bureau à distance"},
                {"es","Datos de escritorio remoto"},
                {"ja","リモート デスクトップ データ"},
                {"hi","दूरस्थ डेस्कटॉप डेटा"},
                {"zh-chs","远程桌面数据"},
                {"de","Remotedesktopdaten"}
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
                {"ko","등록된 이메일 주소로 토큰을 보내시겠습니까?"},
                {"ru","Отправить токен на зарегистрированный адрес электронной почты?"}
            }
        },
        {
            "Application Name",
            new Dictionary<string, string>() {
                {"ko","アプリケーション名"},
                {"fr","Nom de l'application"},
                {"es","Nombre de la aplicación"},
                {"ja","アプリケーション名"},
                {"hi","आवेदन का नाम"},
                {"zh-chs","应用名称"},
                {"de","Anwendungsname"}
            }
        },
        {
            "Error Message",
            new Dictionary<string, string>() {
                {"ko","エラーメッセージ"},
                {"fr","Message d'erreur"},
                {"es","Mensaje de error"},
                {"ja","エラーメッセージ"},
                {"hi","त्रुटि संदेश"},
                {"zh-chs","错误信息"},
                {"de","Fehlermeldung"}
            }
        },
        {
            " MeshCentral Router",
            new Dictionary<string, string>() {
                {"ko"," MeshCentral ルーター"},
                {"fr"," Routeur MeshCentral"},
                {"es"," Enrutador MeshCentral"},
                {"ja"," MeshCentral ルーター"},
                {"hi"," मेशसेंट्रल राउटर"},
                {"zh-chs"," MeshCentral 路由器"},
                {"de"," MeshCentral-Router"}
            }
        },
        {
            "Swap Mouse Buttons",
            new Dictionary<string, string>() {
                {"de","Tauschen Sie die Maustasten aus"},
                {"hi","माउस माउस को स्वैप करें"},
                {"fr","Echanger les boutons de la souris"},
                {"zh-cht","交換鼠標按鈕"},
                {"zh-chs","交换鼠标按钮"},
                {"fi","Vaihda hiiren painikkeet"},
                {"tr","Fare Düğmelerini Değiştirin"},
                {"cs","Zaměňte tlačítka myši"},
                {"ja","マウスボタンを交換する"},
                {"es","Cambiar botones del mouse"},
                {"pt","Botões de troca do mouse"},
                {"nl","Wissel muisknoppen"},
                {"ko","마우스 버튼 교체"},
                {"ru","Поменять местами кнопки мыши"}
            }
        },
        {
            "Change remote desktop settings",
            new Dictionary<string, string>() {
                {"ko","リモート デスクトップの設定を変更する"},
                {"fr","Modifier les paramètres du bureau à distance"},
                {"es","Cambiar la configuración del escritorio remoto"},
                {"ja","リモート デスクトップの設定を変更する"},
                {"hi","दूरस्थ डेस्कटॉप सेटिंग बदलें"},
                {"zh-chs","更改远程桌面设置"},
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
                {"ko","전달(Relay)"},
                {"ru","Ретранслятор"}
            }
        },
        {
            "Remote Desktop Stats",
            new Dictionary<string, string>() {
                {"ko","リモート デスクトップ統計"},
                {"fr","Statistiques du bureau à distance"},
                {"es","Estadísticas de escritorio remoto"},
                {"ja","リモート デスクトップ統計"},
                {"hi","दूरस्थ डेस्कटॉप आँकड़े"},
                {"zh-chs","远程桌面统计"},
                {"de","Remotedesktop-Statistiken"}
            }
        },
        {
            "Alternative Port",
            new Dictionary<string, string>() {
                {"ko","代替ポート"},
                {"fr","Port alternatif"},
                {"es","Puerto alternativo"},
                {"ja","代替ポート"},
                {"hi","वैकल्पिक बंदरगाह"},
                {"zh-chs","替代端口"},
                {"de","Alternativer Hafen"}
            }
        },
        {
            "Incoming Bytes",
            new Dictionary<string, string>() {
                {"ko","受信バイト"},
                {"fr","Octets entrants"},
                {"es","Bytes entrantes"},
                {"ja","受信バイト"},
                {"hi","आने वाली बाइट्स"},
                {"zh-chs","传入字节"},
                {"de","Eingehende Bytes"}
            }
        },
        {
            "Use Alternate Port...",
            new Dictionary<string, string>() {
                {"ko","代替ポートを使用..."},
                {"fr","Utiliser un autre port..."},
                {"es","Usar puerto alternativo ..."},
                {"ja","代替ポートを使用..."},
                {"hi","वैकल्पिक पोर्ट का उपयोग करें..."},
                {"zh-chs","使用备用端口..."},
                {"de","Alternativer Port verwenden..."}
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
                {"ko","프로토콜"},
                {"ru","Протокол"}
            }
        },
        {
            "Send Ctrl-Alt-Del to remote device",
            new Dictionary<string, string>() {
                {"ko","Ctrl-Alt-Del をリモート デバイスに送信する"},
                {"fr","Envoyer Ctrl-Alt-Suppr à l'appareil distant"},
                {"es","Enviar Ctrl-Alt-Del al dispositivo remoto"},
                {"ja","Ctrl-Alt-Del をリモート デバイスに送信する"},
                {"hi","रिमोट डिवाइस पर Ctrl-Alt-Del भेजें"},
                {"zh-chs","发送 Ctrl-Alt-Del 到远程设备"},
                {"de","Strg-Alt-Entf an Remote-Gerät senden"}
            }
        },
        {
            "Routing Status",
            new Dictionary<string, string>() {
                {"ko","ルーティング ステータス"},
                {"fr","État du routage"},
                {"es","Estado de enrutamiento"},
                {"ja","ルーティング ステータス"},
                {"hi","रूटिंग स्थिति"},
                {"zh-chs","路由状态"},
                {"de","Routing-Status"}
            }
        },
        {
            "Sort by G&roup",
            new Dictionary<string, string>() {
                {"ko","グループで並べ替え(&L)"},
                {"fr","Trier par groupe"},
                {"es","Ordenar por grupo y grupo"},
                {"ja","グループで並べ替え(&L)"},
                {"hi","समूह के आधार पर छाँटें"},
                {"zh-chs","按组(&O) 排序"},
                {"de","Nach Gruppe sortieren"}
            }
        },
        {
            "Remove 1 item?",
            new Dictionary<string, string>() {
                {"ko","1 件削除しますか?"},
                {"fr","Supprimer 1 élément ?"},
                {"es","¿Eliminar 1 artículo?"},
                {"ja","1 件削除しますか?"},
                {"hi","1 आइटम निकालें?"},
                {"zh-chs","删除 1 项？"},
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
                {"ko","로컬"},
                {"ru","Локальный"}
            }
        },
        {
            "Local - {0}",
            new Dictionary<string, string>() {
                {"ko","ローカル - {0}"},
                {"fr","Locale - {0}"},
                {"es","Local: {0}"},
                {"ja","ローカル - {0}"},
                {"hi","स्थानीय - {0}"},
                {"zh-chs","本地 - {0}"},
                {"de","Lokal - {0}"}
            }
        },
        {
            "Unable to bind to local port",
            new Dictionary<string, string>() {
                {"ko","ローカル ポートにバインドできません"},
                {"fr","Impossible de se lier au port local"},
                {"es","No se puede vincular al puerto local"},
                {"ja","ローカル ポートにバインドできません"},
                {"hi","स्थानीय पोर्ट से जुड़ने में असमर्थ"},
                {"zh-chs","无法绑定到本地端口"},
                {"de","Kann nicht an lokalen Port binden"}
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
                {"ko","로그인"},
                {"ru","Войти"}
            }
        },
        {
            "Languages",
            new Dictionary<string, string>() {
                {"ko","言語"},
                {"fr","Langues"},
                {"es","Idiomas"},
                {"ja","言語"},
                {"hi","बोली"},
                {"zh-chs","语言"},
                {"de","Sprachen"}
            }
        },
        {
            "Ctrl-Alt-Del",
            new Dictionary<string, string>() {
                {"de","Strg-Alt-Entf"}
            }
        },
        {
            "Port Mapping Help",
            new Dictionary<string, string>() {
                {"ko","ポート マッピング ヘルプ"},
                {"fr","Aide sur le mappage de ports"},
                {"es","Ayuda de mapeo de puertos"},
                {"ja","ポート マッピング ヘルプ"},
                {"hi","पोर्ट मैपिंग सहायता"},
                {"zh-chs","端口映射帮助"},
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
                {"ko","아주 느린"},
                {"ru","Очень медленно"}
            }
        },
        {
            "WARNING - Invalid Server Certificate",
            new Dictionary<string, string>() {
                {"ko","警告 - 無効なサーバー証明書"},
                {"fr","AVERTISSEMENT - Certificat de serveur non valide"},
                {"es","ADVERTENCIA: certificado de servidor no válido"},
                {"ja","警告 - 無効なサーバー証明書"},
                {"hi","चेतावनी - अमान्य सर्वर प्रमाणपत्र"},
                {"zh-chs","警告 - 服务器证书无效"},
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
                {"ko","설치..."},
                {"ru","Установка..."}
            }
        },
        {
            "Remember this certificate",
            new Dictionary<string, string>() {
                {"ko","この証明書を覚えておいてください"},
                {"fr","Rappelez-vous ce certificat"},
                {"es","Recuerda este certificado"},
                {"ja","この証明書を覚えておいてください"},
                {"hi","यह प्रमाणपत्र याद रखें"},
                {"zh-chs","记住这个证书"},
                {"de","Merken Sie sich dieses Zertifikat"}
            }
        },
        {
            "Enter the second factor authentication token.",
            new Dictionary<string, string>() {
                {"ko","第二要素認証トークンを入力します。"},
                {"fr","Saisissez le jeton d'authentification du deuxième facteur."},
                {"es","Ingrese el token de autenticación de segundo factor."},
                {"ja","第二要素認証トークンを入力します。"},
                {"hi","दूसरा कारक प्रमाणीकरण टोकन दर्ज करें।"},
                {"zh-chs","输入第二个因素身份验证令牌。"},
                {"de","Geben Sie das zweite Faktor-Authentifizierungstoken ein."}
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
                {"ko","느린"},
                {"ru","Медленно"}
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
                {"ko","원격"},
                {"ru","Удаленно"}
            }
        },
        {
            "S&ettings...",
            new Dictionary<string, string>() {
                {"ko","設定..."},
                {"fr","Paramètres..."},
                {"es","Ajustes..."},
                {"ja","設定..."},
                {"hi","समायोजन..."},
                {"zh-chs","设置(&E)..."},
                {"de","Die Einstellungen..."}
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
                {"ko","설정"},
                {"ru","Настройки"}
            }
        },
        {
            "MeshCentral Router allows mapping of TCP and UDP ports on this computer to any computer in your MeshCentral server account. Start by logging into your account.",
            new Dictionary<string, string>() {
                {"ko","MeshCentral Router を使用すると、このコンピューターの TCP および UDP ポートを、MeshCentral サーバー アカウント内の任意のコンピューターにマッピングできます。アカウントにログインすることから始めます。"},
                {"fr","Le routeur MeshCentral permet de mapper les ports TCP et UDP de cet ordinateur sur n'importe quel ordinateur de votre compte de serveur MeshCentral. Commencez par vous connecter à votre compte."},
                {"es","MeshCentral Router permite la asignación de puertos TCP y UDP en esta computadora a cualquier computadora en su cuenta de servidor MeshCentral. Empiece por iniciar sesión en su cuenta."},
                {"ja","MeshCentral Router を使用すると、このコンピューターの TCP および UDP ポートを、MeshCentral サーバー アカウント内の任意のコンピューターにマッピングできます。アカウントにログインすることから始めます。"},
                {"hi","MeshCentral राउटर इस कंप्यूटर पर आपके MeshCentral सर्वर खाते के किसी भी कंप्यूटर पर TCP और UDP पोर्ट की मैपिंग की अनुमति देता है। अपने खाते में लॉग इन करके प्रारंभ करें।"},
                {"zh-chs","MeshCentral 路由器允许将此计算机上的 TCP 和 UDP 端口映射到您的 MeshCentral 服务器帐户中的任何计算机。首先登录您的帐户。"},
                {"de","MeshCentral Router ermöglicht die Zuordnung von TCP- und UDP-Ports auf diesem Computer zu jedem Computer in Ihrem MeshCentral-Serverkonto. Melden Sie sich zunächst bei Ihrem Konto an."}
            }
        },
        {
            "Invalid download.",
            new Dictionary<string, string>() {
                {"ko","ダウンロードが無効です。"},
                {"fr","Téléchargement non valide."},
                {"es","Descarga no válida."},
                {"ja","ダウンロードが無効です。"},
                {"hi","अमान्य डाउनलोड।"},
                {"zh-chs","下载无效。"},
                {"de","Ungültiger Download."}
            }
        },
        {
            ", {0} users",
            new Dictionary<string, string>() {
                {"ko","、{0}人のユーザー"},
                {"fr",", {0} utilisateurs"},
                {"es",", {0} usuarios"},
                {"ja","、{0}人のユーザー"},
                {"hi",", {0} उपयोगकर्ता"},
                {"zh-chs",", {0} 个用户"},
                {"de",", {0} Nutzer"}
            }
        },
        {
            "This MeshCentral Server uses a different version of this tool. Click ok to download and update.",
            new Dictionary<string, string>() {
                {"ko","この MeshCentral サーバーは、このツールの異なるバージョンを使用しています。 [OK] をクリックしてダウンロードして更新します。"},
                {"fr","Ce serveur MeshCentral utilise une version différente de cet outil. Cliquez sur ok pour télécharger et mettre à jour."},
                {"es","Este servidor MeshCentral utiliza una versión diferente de esta herramienta. Haga clic en Aceptar para descargar y actualizar."},
                {"ja","この MeshCentral サーバーは、このツールの異なるバージョンを使用しています。 [OK] をクリックしてダウンロードして更新します。"},
                {"hi","यह MeshCentral सर्वर इस टूल के भिन्न संस्करण का उपयोग करता है। डाउनलोड और अपडेट करने के लिए ओके पर क्लिक करें।"},
                {"zh-chs","此 MeshCentral Server 使用此工具的不同版本。单击“确定”进行下载和更新。"},
                {"de","Dieser MeshCentral Server verwendet eine andere Version dieses Tools. Klicken Sie auf OK, um herunterzuladen und zu aktualisieren."}
            }
        },
        {
            "Cancel Auto-Close",
            new Dictionary<string, string>() {
                {"ko","オートクローズをキャンセル"},
                {"fr","Annuler la fermeture automatique"},
                {"es","Cancelar cierre automático"},
                {"ja","オートクローズをキャンセル"},
                {"hi","रद्द करें स्वतः बंद"},
                {"zh-chs","取消自动关闭"},
                {"de","Automatisches Schließen abbrechen"}
            }
        },
        {
            "Open Source, Apache 2.0 License",
            new Dictionary<string, string>() {
                {"ko","オープンソース、Apache 2.0 ライセンス"},
                {"fr","Open Source, licence Apache 2.0"},
                {"es","Código abierto, licencia Apache 2.0"},
                {"ja","オープンソース、Apache 2.0 ライセンス"},
                {"hi","ओपन सोर्स, अपाचे 2.0 लाइसेंस"},
                {"zh-chs","开源，Apache 2.0 许可"},
                {"de","Open Source, Apache 2.0-Lizenz"}
            }
        },
        {
            "Mappings",
            new Dictionary<string, string>() {
                {"ko","マッピング"},
                {"fr","Mappages"},
                {"es","Mapeos"},
                {"ja","マッピング"},
                {"hi","मानचित्रण"},
                {"zh-chs","映射"},
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
                {"ko","여러 장치"},
                {"ru","Устройства"}
            }
        },
        {
            "(Individual Devices)",
            new Dictionary<string, string>() {
                {"ko","(個別デバイス)"},
                {"fr","(Appareils individuels)"},
                {"es","(Dispositivos individuales)"},
                {"ja","(個別デバイス)"},
                {"hi","(व्यक्तिगत उपकरण)"},
                {"zh-chs","（个别设备）"},
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
                {"ko","프라이버시 바"},
                {"ru","Панель конфиденциальности"}
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
                {"ko","개조하다"},
                {"ru","Обновить"}
            }
        },
        {
            "This server presented a un-trusted certificate.  This may indicate that this is not the correct server or that the server does not have a valid certificate. It is not recommanded, but you can press the ignore button to continue connection to this server.",
            new Dictionary<string, string>() {
                {"ko","このサーバーは、信頼できない証明書を提示しました。これは、これが正しいサーバーでないか、サーバーに有効な証明書がないことを示している可能性があります。これは推奨されませんが、無視ボタンを押してこのサーバーへの接続を続けることができます。"},
                {"fr","Ce serveur a présenté un certificat non approuvé. Cela peut indiquer qu'il ne s'agit pas du bon serveur ou que le serveur n'a pas de certificat valide. Ce n'est pas recommandé, mais vous pouvez appuyer sur le bouton ignorer pour continuer la connexion à ce serveur."},
                {"es","Este servidor presentó un certificado no confiable. Esto puede indicar que este no es el servidor correcto o que el servidor no tiene un certificado válido. No se recomienda, pero puede presionar el botón ignorar para continuar la conexión a este servidor."},
                {"ja","このサーバーは、信頼できない証明書を提示しました。これは、これが正しいサーバーでないか、サーバーに有効な証明書がないことを示している可能性があります。これは推奨されませんが、無視ボタンを押してこのサーバーへの接続を続けることができます。"},
                {"hi","इस सर्वर ने एक अविश्वसनीय प्रमाणपत्र प्रस्तुत किया। यह संकेत दे सकता है कि यह सही सर्वर नहीं है या सर्वर के पास वैध प्रमाणपत्र नहीं है। यह अनुशंसित नहीं है, लेकिन आप इस सर्वर से कनेक्शन जारी रखने के लिए अनदेखा करें बटन दबा सकते हैं।"},
                {"zh-chs","此服务器提供了不受信任的证书。这可能表明这不是正确的服务器或服务器没有有效的证书。不推荐，但您可以按忽略按钮继续连接到此服务器。"},
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
                {"ko","암호"},
                {"ru","Пароль"}
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
                {"ko","모든 디스플레이"},
                {"ru","Все экраны"}
            }
        },
        {
            "Double Click Action",
            new Dictionary<string, string>() {
                {"ko","ダブルクリックアクション"},
                {"fr","Action de double-clic"},
                {"es","Acción de doble clic"},
                {"ja","ダブルクリックアクション"},
                {"hi","डबल क्लिक एक्शन"},
                {"zh-chs","双击操作"},
                {"de","Doppelklick-Aktion"}
            }
        },
        {
            "Display connection statistics",
            new Dictionary<string, string>() {
                {"ko","接続統計の表示"},
                {"fr","Afficher les statistiques de connexion"},
                {"es","Mostrar estadísticas de conexión"},
                {"ja","接続統計の表示"},
                {"hi","कनेक्शन आंकड़े प्रदर्शित करें"},
                {"zh-chs","显示连接统计"},
                {"de","Verbindungsstatistik anzeigen"}
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
                {"ko","연결 중 ..."},
                {"ru","Подключение..."}
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
                {"ko","연결"},
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
            "{0} Byte",
            new Dictionary<string, string>() {
                {"ko","{0}バイト"},
                {"fr","{0} octet"},
                {"ja","{0}バイト"},
                {"hi","{0} बाइट"},
                {"zh-chs","{0} 字节"}
            }
        },
        {
            "Remote Device",
            new Dictionary<string, string>() {
                {"ko","リモートデバイス"},
                {"fr","Périphérique distant"},
                {"es","Dispositivo remoto"},
                {"ja","リモートデバイス"},
                {"hi","रिमोट डिवाइस"},
                {"zh-chs","遥控装置"},
                {"de","Remote-Gerät"}
            }
        },
        {
            "HTTPS",
            new Dictionary<string, string>() {
                {"hi","HTTPS के"}
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
                {"ko","오프라인"},
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
                {"ko","타임 아웃"},
                {"ru","Тайм-аут"}
            }
        },
        {
            "Push local clipboard to remote device",
            new Dictionary<string, string>() {
                {"ko","ローカル クリップボードをリモート デバイスにプッシュ"},
                {"fr","Transférer le presse-papiers local vers l'appareil distant"},
                {"es","Empuje el portapapeles local al dispositivo remoto"},
                {"ja","ローカル クリップボードをリモート デバイスにプッシュ"},
                {"hi","स्थानीय क्लिपबोर्ड को दूरस्थ डिवाइस पर पुश करें"},
                {"zh-chs","将本地剪贴板推送到远程设备"},
                {"de","Lokale Zwischenablage auf Remote-Gerät übertragen"}
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
                {"ko","연결 해제"},
                {"ru","Разъединить"}
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
                {"ko","잘못된 네트워크 데이터를 받았습니다."},
                {"ru","Получены неверные сетевые данные"}
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
                {"ko","원격 데스크톱 설정"},
                {"ru","Настройки удаленного рабочего стола"}
            }
        },
        {
            "MeshCentral Router Installation",
            new Dictionary<string, string>() {
                {"ko","MeshCentral ルーターのインストール"},
                {"fr","Installation du routeur MeshCentral"},
                {"es","Instalación del enrutador MeshCentral"},
                {"ja","MeshCentral ルーターのインストール"},
                {"hi","मेशसेंट्रल राउटर इंस्टालेशन"},
                {"zh-chs","MeshCentral 路由器安装"},
                {"de","Installation des MeshCentral-Routers"}
            }
        },
        {
            "Stopped.",
            new Dictionary<string, string>() {
                {"ko","停止。"},
                {"fr","Arrêté."},
                {"es","Detenido."},
                {"ja","停止。"},
                {"hi","रोका हुआ।"},
                {"zh-chs","停了。"},
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
