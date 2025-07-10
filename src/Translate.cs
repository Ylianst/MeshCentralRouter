/*
Copyright 2009-2022 Intel Corporation

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
        // Do not edit the translations below, instead change the master "translate.json" file of MeshCentral.
        // This file is auto-generated from the "translate.json" file.

        // *** TRANSLATION TABLE START ***
        static private Dictionary<string, Dictionary<string, string>> translationTable = new Dictionary<string, Dictionary<string, string>>() {
        {
            "Site",
            new Dictionary<string, string>() {
                {"de","Seite? ˅"},
                {"hi","साइट"},
                {"fr","Placer"},
                {"zh-chs","地点"},
                {"fi","Sivusto"},
                {"tr","Alan"},
                {"cs","Stránky"},
                {"ja","サイト"},
                {"es","Sitio"},
                {"pl","Strona"},
                {"pt","Local"},
                {"nl","Lokatie"},
                {"pt-br","Local"},
                {"sv","Webbplats"},
                {"da","Websted"},
                {"ko","대지"},
                {"it","Sito"},
                {"ru","Сайт"}
            }
        },
        {
            ", Recorded Session",
            new Dictionary<string, string>() {
                {"de",", Aufgezeichnete Sitzung"},
                {"hi",", रिकॉर्ड किया गया सत्र"},
                {"fr",", Séance enregistrée"},
                {"zh-chs",", 录制会话"},
                {"fi",", Tallennettu istunto"},
                {"tr",", Kaydedilmiş Oturum"},
                {"cs",", Nahraná relace"},
                {"ja","、記録されたセッション"},
                {"es",", Sesión grabada"},
                {"pl",", Nagrana Sesja"},
                {"pt",", Sessão Gravada"},
                {"nl",", opgenomen sessie"},
                {"pt-br",", Sessão Gravada"},
                {"sv",", Inspelad session"},
                {"da",", Optaget session"},
                {"ko",", 녹화 된 세션"},
                {"it",", Sessione registrata"},
                {"ru",", Записанный сеанс"}
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
                {"pl","Wszystkie Ekrany"},
                {"pt","Todas as telas"},
                {"nl","Alle schermen"},
                {"pt-br","Todos os monitores"},
                {"sv","Alla skärmar"},
                {"da","Alle skærme"},
                {"ko","모든 디스플레이"},
                {"it","Tutti i display"},
                {"ru","Все экраны"}
            }
        },
        {
            "MeshCentral",
            new Dictionary<string, string>() {
                {"hi","मेशसेंट्रल"},
                {"zh-chs","网格中心"}
            }
        },
        {
            "Incoming Bytes",
            new Dictionary<string, string>() {
                {"de","Eingehende Bytes"},
                {"hi","आने वाली बाइट्स"},
                {"fr","Octets entrants"},
                {"zh-chs","传入字节"},
                {"fi","Saapuvat tavut"},
                {"tr","Gelen Bayt"},
                {"cs","Příchozí bajty"},
                {"ja","着信バイト"},
                {"es","Bytes entrantes"},
                {"pl","Bajty Przychodzące"},
                {"pt","Bytes de entrada"},
                {"nl","Inkomende Bytes"},
                {"pt-br","Bytes de entrada"},
                {"sv","Inkommande byte"},
                {"da","Indkommende byte"},
                {"ko","들어오는 바이트"},
                {"it","Byte in entrata"},
                {"ru","Входящие байты"}
            }
        },
        {
            "Invalid download.",
            new Dictionary<string, string>() {
                {"de","Ungültiger Download."},
                {"hi","अमान्य डाउनलोड।"},
                {"fr","Téléchargement non valide."},
                {"zh-chs","下载无效。"},
                {"fi","Virheellinen lataus."},
                {"tr","Geçersiz indirme."},
                {"cs","Neplatné stahování."},
                {"ja","ダウンロードが無効です。"},
                {"es","Descarga no válida."},
                {"pl","Nieprawidłowe pobieranie."},
                {"pt","Download inválido."},
                {"nl","Ongeldige download."},
                {"pt-br","Download inválido."},
                {"sv","Ogiltig nedladdning."},
                {"da","Ugyldig download."},
                {"ko","다운로드가 잘못되었습니다."},
                {"it","Download non valido."},
                {"ru","Неверная загрузка."}
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
                {"tr","Yayın"},
                {"cs","Předávání (relay)"},
                {"ja","リレー"},
                {"es","Relé"},
                {"pl","Przekierowanie"},
                {"pt","Retransmissão"},
                {"pt-br","Retransmissão"},
                {"sv","Relä"},
                {"ko","전달(Relay)"},
                {"it","Ritrasmissioni"},
                {"ru","Ретранслятор"}
            }
        },
        {
            "Port {0} to {1}:{2}",
            new Dictionary<string, string>() {
                {"de","Port {0} nach {1}:{2}"},
                {"hi","पोर्ट {0} से {1}:{2}"},
                {"fr","Port {0} vers {1} :{2}"},
                {"zh-chs","端口 {0} 到 {1}：{2}"},
                {"fi","Portti {0} kohteeseen {1}: {2}"},
                {"tr","{0} bağlantı noktası {1}:{2}"},
                {"cs","Port {0} do {1}: {2}"},
                {"ja","ポート{0}から{1}：{2}"},
                {"es","Puerto {0} a {1}: {2}"},
                {"pl","Port {0} do {1}:{2}"},
                {"pt","Porta {0} para {1}: {2}"},
                {"nl","Poort {0} naar {1}:{2}"},
                {"pt-br","Porta {0} para {1}: {2}"},
                {"sv","Port {0} till {1}: {2}"},
                {"da","Port {0} til {1}:{2}"},
                {"ko","{0}에서 {1}로 포트 : {2}"},
                {"it","Porta {0} a {1}:{2}"},
                {"ru","Порт {0} в {1}: {2}"}
            }
        },
        {
            "Remote - {0}",
            new Dictionary<string, string>() {
                {"de","Fernbedienung - {0}"},
                {"hi","रिमोट - {0}"},
                {"fr","À distance - {0}"},
                {"zh-chs","远程 - {0}"},
                {"fi","Kaukosäädin - {0}"},
                {"tr","Uzak - {0}"},
                {"cs","Dálkové ovládání - {0}"},
                {"ja","リモート-{0}"},
                {"es","Remoto: {0}"},
                {"pl","Zdalny - {0}"},
                {"pt","Remoto - {0}"},
                {"nl","Afstandsbediening - {0}"},
                {"pt-br","Remoto - {0}"},
                {"sv","Fjärrkontroll - {0}"},
                {"ko","원격-{0}"},
                {"it","Remoto - {0}"},
                {"ru","Удаленный - {0}"}
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
                {"pl","Bardzo powoli"},
                {"pt","Muito devagar"},
                {"nl","Erg traag"},
                {"pt-br","Muito lento"},
                {"sv","Väldigt långsam"},
                {"da","Meget langsom"},
                {"ko","아주 느린"},
                {"it","Molto lento"},
                {"ru","Очень медленно"}
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
            "Confirm Delete",
            new Dictionary<string, string>() {
                {"de","Löschen bestätigen"},
                {"hi","हटाने की पुष्टि करें"},
                {"fr","Confirmation de la suppression"},
                {"zh-chs","确认删除"},
                {"fi","Vahvista poistaminen"},
                {"tr","Silmeyi Onayla"},
                {"cs","Potvrďte smazání"},
                {"ja","削除の確認"},
                {"es","Confirmar Eliminación"},
                {"pl","Potwierdzić Usunięcie"},
                {"pt","Confirmar exclusão"},
                {"nl","Verwijderen bevestigen"},
                {"pt-br","Confirmar exclusão"},
                {"sv","Bekräfta radering"},
                {"da","Bekræft sletning"},
                {"ko","삭제 확인"},
                {"it","Conferma cancellazione"},
                {"ru","Подтвердите удаление"}
            }
        },
        {
            "Estimating...",
            new Dictionary<string, string>() {
                {"de","Schätzung..."},
                {"hi","अनुमान लगाया जा रहा है..."},
                {"fr","Estimation ..."},
                {"zh-chs","估计..."},
                {"fi","Arvioidaan ..."},
                {"tr","tahmin ediliyor..."},
                {"cs","Odhadování ..."},
                {"ja","見積もり..."},
                {"es","Estimando ..."},
                {"pl","Doceniający..."},
                {"pt","Estimando ..."},
                {"nl","geschat..."},
                {"pt-br","Estimando..."},
                {"sv","Uppskattar ..."},
                {"da","Estimerer..."},
                {"ko","추정 중..."},
                {"it","Stima..."},
                {"ru","Оценка ..."}
            }
        },
        {
            "Remote IP",
            new Dictionary<string, string>() {
                {"de","Remote-IP"},
                {"hi","दूरदराज़ के आई. पी"},
                {"fr","IP distante"},
                {"zh-chs","远程IP"},
                {"fi","Etä -IP"},
                {"tr","uzak IP"},
                {"cs","Vzdálená IP"},
                {"ja","リモートIP"},
                {"es","IP Remota"},
                {"pl","Zdalny IP"},
                {"pt","IP Remoto"},
                {"nl","Extern IP"},
                {"pt-br","IP Remoto"},
                {"sv","Fjärr-IP"},
                {"da","Fjern-IP"},
                {"ko","원격 IP"},
                {"it","IP remoto"},
                {"ru","Удаленный IP"}
            }
        },
        {
            "Open Web Site",
            new Dictionary<string, string>() {
                {"de","Website öffnen"},
                {"hi","वेब साइट खोलें"},
                {"fr","Ouvrir le site Web"},
                {"zh-chs","打开网站"},
                {"fi","Avaa verkkosivusto"},
                {"tr","Web Sitesini Aç"},
                {"cs","Otevřete web"},
                {"ja","Webサイトを開く"},
                {"es","Abrir Sitio Web"},
                {"pl","Otwórz Stronę"},
                {"pt","Abra o site"},
                {"nl","Website openen"},
                {"pt-br","Abra o site"},
                {"sv","Öppna webbplatsen"},
                {"da","Åbn webstedet"},
                {"ko","웹 사이트 열기"},
                {"it","Apri sito web"},
                {"ru","Открыть веб-сайт"}
            }
        },
        {
            "ComputerName",
            new Dictionary<string, string>() {
                {"de","Computername"},
                {"hi","कंप्यूटर का नाम"},
                {"fr","Nom de l'ordinateur"},
                {"zh-chs","计算机名"},
                {"fi","Tietokoneen nimi"},
                {"tr","Bilgisayar adı"},
                {"cs","Název počítače"},
                {"ja","コンピュータネーム"},
                {"es","Nombre de la Computadora"},
                {"pl","Nazwa komputera"},
                {"pt","Nome do computador"},
                {"nl","Computernaam"},
                {"pt-br","Nome do computador"},
                {"sv","Datornamn"},
                {"da","Computernavn"},
                {"it","Nome del computer"},
                {"ru","Имя компьютера"}
            }
        },
        {
            "Create Folder",
            new Dictionary<string, string>() {
                {"de","Ordner erstellen"},
                {"hi","फोल्डर बनाएं"},
                {"fr","Créer le dossier"},
                {"zh-chs","创建文件夹"},
                {"fi","Luo kansio"},
                {"tr","Klasör oluşturun"},
                {"cs","Vytvořit složku"},
                {"ja","フォルダーを作る"},
                {"es","Crear Carpeta"},
                {"pl","Utwórz Folder"},
                {"pt","Criar pasta"},
                {"nl","Map aanmaken"},
                {"pt-br","Criar pasta"},
                {"sv","Skapa mapp"},
                {"da","Opret mappe"},
                {"ko","폴더 생성"},
                {"it","Creare una cartella"},
                {"ru","Создать папку"}
            }
        },
        {
            "Cancel Auto-Close",
            new Dictionary<string, string>() {
                {"de","Automatisches Schließen abbrechen"},
                {"hi","रद्द करें स्वतः बंद"},
                {"fr","Annuler la fermeture automatique"},
                {"zh-chs","取消自动关闭"},
                {"fi","Peruuta automaattinen sulkeminen"},
                {"tr","Otomatik Kapatmayı İptal Et"},
                {"cs","Zrušit automatické zavírání"},
                {"ja","自動クローズをキャンセルする"},
                {"es","Cancelar Cierre Automático"},
                {"pl","Anuluj Auto-Zamknięcie"},
                {"pt","Cancelar fechamento automático"},
                {"nl","Automatisch sluiten annuleren"},
                {"pt-br","Cancelar fechamento automático"},
                {"sv","Avbryt automatisk stängning"},
                {"da","Annuller auto-luk"},
                {"ko","자동 닫기 취소"},
                {"it","Annulla chiusura automatica"},
                {"ru","Отменить автоматическое закрытие"}
            }
        },
        {
            ", 1 connection.",
            new Dictionary<string, string>() {
                {"de",", 1 Verbindung."},
                {"hi",", 1 कनेक्शन।"},
                {"fr",", 1 connexion."},
                {"zh-chs",", 1 个连接。"},
                {"fi",", 1 liitäntä."},
                {"tr",", 1 bağlantı."},
                {"cs",", 1 připojení."},
                {"ja","、1接続。"},
                {"es",", 1 conexión."},
                {"pl",", 1 połączenie."},
                {"pt",", 1 conexão."},
                {"nl",", 1 verbinding."},
                {"pt-br",", 1 conexão."},
                {"sv",", 1 anslutning."},
                {"da",", 1 forbindelse."},
                {"ko",", 1 촌."},
                {"it",", 1 connessione."},
                {"ru",", 1 соединение."}
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
                {"pl","Łączenie..."},
                {"pt","Conectando..."},
                {"nl","Verbinden..."},
                {"pt-br","Conectando ..."},
                {"sv","Ansluter..."},
                {"da","Forbinder..."},
                {"ko","연결 중 ..."},
                {"it","Collegamento ..."},
                {"ru","Подключение..."}
            }
        },
        {
            "&Open Mappings...",
            new Dictionary<string, string>() {
                {"de","&Zuordnungen öffnen..."},
                {"hi","&ओपन मैपिंग..."},
                {"fr","&Ouvrir les mappages..."},
                {"zh-chs","打开映射 (&O)..."},
                {"fi","& Avaa kartoitukset ..."},
                {"tr","&Eşlemeleri Aç..."},
                {"cs","& Otevřít mapování ..."},
                {"ja","＆Open Mapping .. .."},
                {"es","&Abrir mapeos ..."},
                {"pl","&Otwórz Mapowania..."},
                {"pt","& Abrir mapeamentos ..."},
                {"nl","&toewijzingen openen..."},
                {"pt-br","&Abrir mapeamentos ..."},
                {"sv","& Öppna kartläggningar ..."},
                {"ko","매핑 열기 ..."},
                {"it","&Apri mappature..."},
                {"ru","&Открыть, сопоставить..."}
            }
        },
        {
            "TCP",
            new Dictionary<string, string>() {
                {"hi","टीसीपी"}
            }
        },
        {
            "Remote Desktop Data",
            new Dictionary<string, string>() {
                {"de","Remotedesktopdaten"},
                {"hi","दूरस्थ डेस्कटॉप डेटा"},
                {"fr","Données de bureau à distance"},
                {"zh-chs","远程桌面数据"},
                {"fi","Etätyöpöydän tiedot"},
                {"tr","Uzak Masaüstü Verileri"},
                {"cs","Data vzdálené plochy"},
                {"ja","リモートデスクトップデータ"},
                {"es","Datos de Escritorio Remoto"},
                {"pl","Dane Pulpitu Zdalnego"},
                {"pt","Dados da área de trabalho remota"},
                {"nl","Extern bureaublad gegevens"},
                {"pt-br","Dados da área de trabalho remota"},
                {"sv","Fjärrskrivbordsdata"},
                {"da","Fjernskrivebordsdata"},
                {"ko","원격 데스크톱 데이터"},
                {"it","Dati desktop remoto"},
                {"ru","Данные удаленного рабочего стола"}
            }
        },
        {
            "Add Relay Map...",
            new Dictionary<string, string>() {
                {"de","Relaiskarte hinzufügen..."},
                {"hi","रिले मैप जोड़ें..."},
                {"fr","Ajouter une carte de relais..."},
                {"zh-chs","添加中继地图..."},
                {"fi","Lisää välityskartta ..."},
                {"tr","Geçiş Haritası Ekle..."},
                {"cs","Přidat mapu relé ..."},
                {"ja","リレーマップを追加..."},
                {"es","Agregar mapa de retransmisiones ..."},
                {"pl","Dodaj Mapę Przekierowania..."},
                {"pt","Adicionar mapa de retransmissão ..."},
                {"nl","Toevoegen relay kaart..."},
                {"pt-br","Adicionar mapa de retransmissão ..."},
                {"sv","Lägg till reläkarta ..."},
                {"da","Tilføj Relay kort"},
                {"ko","릴레이 맵 추가 ..."},
                {"it","Aggiungi mappa di rilancio..."},
                {"ru","Добавить карту реле ..."}
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
                {"pl","Wysłać token na zarejestrowany adres email?"},
                {"pt","Enviar token para o endereço de e-mail registrado?"},
                {"nl","Token verzenden naar geregistreerd e-mailadres?"},
                {"pt-br","Enviar token para o endereço de e-mail registrado?"},
                {"sv","Skicka token till registrerad e-postadress?"},
                {"da","Send token til registreret e-mailadresse?"},
                {"ko","등록된 이메일 주소로 토큰을 보내시겠습니까?"},
                {"it","Invia token all'indirizzo email registrato?"},
                {"ru","Отправить токен на зарегистрированный адрес электронной почты?"}
            }
        },
        {
            "HTTP",
            new Dictionary<string, string>() {
                {"hi","एचटीटीपी"}
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
                {"pl","Zabronione"},
                {"pt","Negado"},
                {"nl","Geweigerd"},
                {"pt-br","Negado"},
                {"sv","Förnekad"},
                {"da","Afslået"},
                {"ko","거부"},
                {"it","Negato"},
                {"ru","Отказано"}
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
                {"pl","Powoli"},
                {"pt","Lento"},
                {"nl","Traag"},
                {"pt-br","Lento"},
                {"sv","Långsam"},
                {"da","Langsom"},
                {"ko","느린"},
                {"it","Lento"},
                {"ru","Медленно"}
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
                {"pl","Pasek Prywatności"},
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
            "Display {0}",
            new Dictionary<string, string>() {
                {"de","Anzeige {0}"},
                {"hi","प्रदर्शन {0}"},
                {"fr","Affichage {0}"},
                {"zh-chs","显示{0}"},
                {"fi","Näyttö {0}"},
                {"tr","{0} göster"},
                {"cs","Zobrazit {0}"},
                {"ja","{0}を表示"},
                {"es","Mostrar {0}"},
                {"pl","Ekran {0}"},
                {"pt","Exibir {0}"},
                {"nl","Scherm {0}"},
                {"pt-br","Exibir {0}"},
                {"sv","Visa {0}"},
                {"da","Vis {0}"},
                {"ko","{0} 디스플레이"},
                {"ru","Экран {0}"}
            }
        },
        {
            "Add &Map...",
            new Dictionary<string, string>() {
                {"de","&Zuordnen hinzufügen..."},
                {"hi","नक्शा जोड़ें..."},
                {"fr","Ajouter une &carte..."},
                {"zh-chs","添加地图 (&M)..."},
                {"fi","Lisää & kartta ..."},
                {"tr","&Harita ekle..."},
                {"cs","Přidat a mapovat ..."},
                {"ja","＆Mapを追加..."},
                {"es","Agregar & mapa ..."},
                {"pl","Dodaj &Mapę..."},
                {"pt","Adicionar & mapear ..."},
                {"nl","&Toewijzing toevoegen..."},
                {"pt-br","Adicionar & mapear ..."},
                {"sv","Lägg till & mappa ..."},
                {"da","Tilføj &Map..."},
                {"ko","지도 추가 ..."},
                {"it","Aggiungi &mappa..."},
                {"ru","Добавить & карту ..."}
            }
        },
        {
            "Error downloading file: {0}",
            new Dictionary<string, string>() {
                {"de","Fehler beim Herunterladen der Datei: {0}"},
                {"hi","फ़ाइल डाउनलोड करने में त्रुटि: {0}"},
                {"fr","Erreur lors du téléchargement du fichier : {0}"},
                {"zh-chs","下载文件时出错：{0}"},
                {"fi","Virhe tiedoston lataamisessa: {0}"},
                {"tr","Dosya indirilirken hata oluştu: {0}"},
                {"cs","Chyba při stahování souboru: {0}"},
                {"ja","ファイルのダウンロード中にエラーが発生しました：{0}"},
                {"es","Error al descargar el archivo: {0}"},
                {"pl","Błąd podczas pobierania pliku: {0}"},
                {"pt","Erro ao fazer download do arquivo: {0}"},
                {"nl","Fout bij downloaden van bestand: {0}"},
                {"pt-br","Erro ao baixar arquivo: {0}"},
                {"sv","Fel vid nedladdning av fil: {0}"},
                {"da","Fejl ved download af fil: {0}"},
                {"ko","파일 다운로드 중 오류: {0}"},
                {"it","Errore durante il download del file: {0}"},
                {"ru","Ошибка при загрузке файла: {0}"}
            }
        },
        {
            "OK",
            new Dictionary<string, string>() {
                {"hi","ठीक"},
                {"fr","ОК"},
                {"tr","Tamam"},
                {"pt","Ok"},
                {"ko","확인"},
                {"ru","ОК"}
            }
        },
        {
            "Port Mapping Help",
            new Dictionary<string, string>() {
                {"de","Hilfe zur Portzuordnung"},
                {"hi","पोर्ट मैपिंग सहायता"},
                {"fr","Aide sur le mappage de ports"},
                {"zh-chs","端口映射帮助"},
                {"fi","Portin kartoitusohje"},
                {"tr","Bağlantı Noktası Eşleme Yardımı"},
                {"cs","Nápověda k mapování portů"},
                {"ja","ポートマッピングヘルプ"},
                {"es","Ayuda de Mapeo de Puertos"},
                {"pl","Pomoc Mapowania Portu"},
                {"pt","Ajuda para mapeamento de portas"},
                {"nl","Hulp bij poorttoewijzing"},
                {"pt-br","Ajuda para mapeamento de portas"},
                {"sv","Portmappning Hjälp"},
                {"da","Port Mapping hjælp"},
                {"ko","포트 매핑 도움말"},
                {"it","Aiuto per la mappatura delle porte"},
                {"ru","Справка по отображению портов"}
            }
        },
        {
            "Show &Offline Devices",
            new Dictionary<string, string>() {
                {"de","&Offline-Geräte anzeigen"},
                {"hi","दिखाएँ &ऑफ़लाइन उपकरण"},
                {"fr","Afficher les appareils &hors ligne"},
                {"zh-chs","显示离线设备 (&A)"},
                {"fi","Näytä ja offline -laitteet"},
                {"tr","&Çevrimdışı Cihazları Göster"},
                {"cs","Zobrazit a offline zařízení"},
                {"ja","デバイスの表示とオフライン"},
                {"es","Mostrar y dispositivos sin conexión"},
                {"pl","Pokaż Urządzenia &Offline"},
                {"pt","Mostrar dispositivos off-line"},
                {"nl","Toon &Offline apparaten"},
                {"pt-br","Mostrar dispositivos off-line"},
                {"sv","Visa & Offline-enheter"},
                {"da","Vis &Offline enheder"},
                {"ko","오프라인 장치 표시 (& O)"},
                {"it","Mostra dispositivi offline"},
                {"ru","Показать и автономные устройства"}
            }
        },
        {
            "MeshCentral Router Update",
            new Dictionary<string, string>() {
                {"de","MeshCentral Router-Update"},
                {"hi","मेशसेंट्रल राउटर अपडेट"},
                {"fr","Mise à jour du routeur MeshCentral"},
                {"zh-chs","MeshCentral 路由器更新"},
                {"fi","MeshCentral -reitittimen päivitys"},
                {"tr","MeshCentral Yönlendirici Güncellemesi"},
                {"cs","Aktualizace routeru MeshCentral"},
                {"ja","MeshCentralルーターの更新"},
                {"es","Actualización del enrutador MeshCentral"},
                {"pl","Aktualizacja MeshCentral Router"},
                {"pt","Atualização do roteador MeshCentral"},
                {"pt-br","Atualização do MeshCentral Router"},
                {"sv","MeshCentral routeruppdatering"},
                {"da","MeshCentral routeropdatering"},
                {"ko","MeshCentral 라우터 업데이트"},
                {"it","Aggiornamento del router MeshCentral"},
                {"ru","Обновление MeshCentral Router"}
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
                {"pl","Rozmiar"},
                {"pt","Tamanho"},
                {"nl","Grootte"},
                {"pt-br","Tamanho"},
                {"sv","Storlek"},
                {"da","Størrelse"},
                {"ko","크기"},
                {"it","Dimensione"},
                {"ru","Размер"}
            }
        },
        {
            "CIRA",
            new Dictionary<string, string>() {
                {"hi","सीआईआरए"}
            }
        },
        {
            "Use Alternate Port...",
            new Dictionary<string, string>() {
                {"de","Alternativer Port verwenden..."},
                {"hi","वैकल्पिक पोर्ट का उपयोग करें..."},
                {"fr","Utiliser un autre port..."},
                {"zh-chs","使用备用端口..."},
                {"fi","Käytä vaihtoehtoista porttia ..."},
                {"tr","Alternatif Bağlantı Noktasını Kullan..."},
                {"cs","Použít alternativní port ..."},
                {"ja","代替ポートを使用..."},
                {"es","Usar Puerto Alternativo ..."},
                {"pl","Użyj Alternatywnego Portu..."},
                {"pt","Usar porta alternativa ..."},
                {"nl","Alternatieve poort gebruiken..."},
                {"pt-br","Usar porta alternativa ..."},
                {"sv","Använd alternativ port ..."},
                {"da","Brug alternativ port..."},
                {"ko","대체 포트 사용 ..."},
                {"it","Usa porta alternativa..."},
                {"ru","Использовать альтернативный порт ..."}
            }
        },
        {
            "Unable to open file: {0}",
            new Dictionary<string, string>() {
                {"de","Datei kann nicht geöffnet werden: {0}"},
                {"hi","फ़ाइल खोलने में असमर्थ: {0}"},
                {"fr","Impossible d'ouvrir le fichier : {0}"},
                {"zh-chs","无法打开文件：{0}"},
                {"fi","Tiedostoa ei voi avata: {0}"},
                {"tr","Dosya açılamıyor: {0}"},
                {"cs","Soubor nelze otevřít: {0}"},
                {"ja","ファイルを開くことができません：{0}"},
                {"es","No se puede abrir el archivo: {0}"},
                {"pl","Nie można otworzyć pliku: {0}"},
                {"pt","Não foi possível abrir o arquivo: {0}"},
                {"nl","Kan bestand: {0} niet openen"},
                {"pt-br","No se puede abrir el archivo: {0}"},
                {"sv","Det gick inte att öppna filen: {0}"},
                {"da","Kan ikke åbne filen: {0}"},
                {"ko","파일을 열 수 없음: {0}"},
                {"it","Impossibile aprire il file: {0}"},
                {"ru","Невозможно открыть файл: {0}"}
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
                {"pl","Połącz"},
                {"pt","Conectar"},
                {"nl","Verbinden"},
                {"pt-br","Conectar"},
                {"sv","Anslut"},
                {"da","Forbind"},
                {"ko","연결"},
                {"it","Connetti"},
                {"ru","Подключиться"}
            }
        },
        {
            "MeshCentral Router Installation",
            new Dictionary<string, string>() {
                {"de","Installation des MeshCentral-Routers"},
                {"hi","मेशसेंट्रल राउटर इंस्टालेशन"},
                {"fr","Installation du routeur MeshCentral"},
                {"zh-chs","MeshCentral 路由器安装"},
                {"fi","MeshCentral -reitittimen asennus"},
                {"tr","MeshCentral Yönlendirici Kurulumu"},
                {"cs","Instalace routeru MeshCentral"},
                {"ja","MeshCentralルーターのインストール"},
                {"es","Instalación del enrutador MeshCentral"},
                {"pl","Instalacja MeshCentral Router"},
                {"pt","Instalação do roteador MeshCentral"},
                {"nl","MeshCentral Router Installatie"},
                {"pt-br","Instalação do MeshCentral Router"},
                {"ko","MeshCentral 라우터 설치"},
                {"it","Installazione del router MeshCentral"},
                {"ru","Установка MeshCentral Router"}
            }
        },
        {
            "Double Click Action",
            new Dictionary<string, string>() {
                {"de","Doppelklick-Aktion"},
                {"hi","डबल क्लिक एक्शन"},
                {"fr","Action de double-clic"},
                {"zh-chs","双击操作"},
                {"fi","Kaksoisnapsauta toimintoa"},
                {"tr","Çift Tıklama İşlemi"},
                {"cs","Akce dvojitým kliknutím"},
                {"ja","ダブルクリックアクション"},
                {"es","Acción de doble clic"},
                {"pl","Akcja Podwójnego Kliknięcia"},
                {"pt","Ação de duplo clique"},
                {"nl","Dubbelklik actie"},
                {"pt-br","Ação de duplo clique"},
                {"sv","Dubbelklicka på åtgärd"},
                {"da","Dobbeltklik på handling"},
                {"ko","더블 클릭 동작"},
                {"it","Azione doppio clic"},
                {"ru","Действие двойного щелчка"}
            }
        },
        {
            "(Individual Devices)",
            new Dictionary<string, string>() {
                {"de","(Einzelgeräte)"},
                {"hi","(व्यक्तिगत उपकरण)"},
                {"fr","(Appareils individuels)"},
                {"zh-chs","（个别设备）"},
                {"fi","(Yksittäiset laitteet)"},
                {"tr","(Bireysel Cihazlar)"},
                {"cs","(Jednotlivá zařízení)"},
                {"ja","（個別のデバイス）"},
                {"es","(Dispositivos individuales)"},
                {"pl","(Poszczególne urządzenia)"},
                {"pt","(Dispositivos Individuais)"},
                {"nl","(Individuele apparaten)"},
                {"pt-br","(Dispositivos Individuais)"},
                {"sv","(Enskilda enheter)"},
                {"da","(Individuelle enheder)"},
                {"ko","(개별 기기)"},
                {"it","(Dispositivi individuali)"},
                {"ru","(Отдельные устройства)"}
            }
        },
        {
            "Path",
            new Dictionary<string, string>() {
                {"de","Pfad"},
                {"hi","पथ"},
                {"fr","Chemin"},
                {"zh-chs","小路"},
                {"fi","Polku"},
                {"tr","Yol"},
                {"cs","Cesta"},
                {"ja","道"},
                {"es","Camino"},
                {"pl","Ścieżka"},
                {"pt","Caminho"},
                {"nl","Pad"},
                {"pt-br","Caminho"},
                {"sv","Väg"},
                {"da","Sti"},
                {"ko","통로"},
                {"it","Percorso"},
                {"ru","Дорожка"}
            }
        },
        {
            "Desktop Settings",
            new Dictionary<string, string>() {
                {"de","Desktop-Einstellungen"},
                {"hi","डेस्कटॉप सेटिंग्स"},
                {"fr","Paramètres du bureau"},
                {"zh-chs","桌面设置"},
                {"fi","Työpöydän asetukset"},
                {"tr","Masaüstü Ayarları"},
                {"cs","Nastavení plochy"},
                {"ja","デスクトップ設定"},
                {"es","Configuración de escritorio"},
                {"pl","Ustawienia Pulpitu"},
                {"pt","Configurações da área de trabalho"},
                {"nl","Bureaubladinstellingen"},
                {"pt-br","Configurações da área de trabalho"},
                {"sv","Skrivbordsinställningar"},
                {"da","Skrivebordsindstillninger"},
                {"ko","데스크탑 설정"},
                {"it","Impostazioni del desktop"},
                {"ru","Настройки рабочего стола"}
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
                {"pl","Ustawienia Pulpitu Zdalnego"},
                {"pt","Configurações da área de trabalho remota"},
                {"nl","Instellingen extern bureaublad"},
                {"pt-br","Configurações de área de trabalho remota"},
                {"sv","Fjärrskrivbordsinställningar"},
                {"da","Indstillinger for fjernskrivebord"},
                {"ko","원격 데스크톱 설정"},
                {"it","Impostazioni desktop remoto"},
                {"ru","Настройки удаленного рабочего стола"}
            }
        },
        {
            "Relay Device",
            new Dictionary<string, string>() {
                {"de","Relaisgerät"},
                {"hi","रिले डिवाइस"},
                {"fr","Dispositif de relais"},
                {"zh-chs","中继装置"},
                {"fi","Relelaite"},
                {"tr","Röle Cihazı"},
                {"cs","Reléové zařízení"},
                {"ja","リレー装置"},
                {"es","Dispositivo de Retransmisión"},
                {"pl","Urządzenie Przekazujące"},
                {"pt","Dispositivo de Relé"},
                {"nl","Doorstuur apparaat"},
                {"pt-br","Dispositivo de Retransmissão"},
                {"sv","Reläenhet"},
                {"da","Relay enhed"},
                {"ko","릴레이 장치"},
                {"it","Dispositivo ripetitore"},
                {"ru","Релейное устройство"}
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
                {"pl","Instalacja..."},
                {"pt","Configurando..."},
                {"pt-br","Configurando..."},
                {"sv","Uppstart..."},
                {"da","Opsætning..."},
                {"ko","설치..."},
                {"it","Impostare..."},
                {"ru","Установка..."}
            }
        },
        {
            "Unable to bind to local port",
            new Dictionary<string, string>() {
                {"de","Kann nicht an lokalen Port binden"},
                {"hi","स्थानीय पोर्ट से जुड़ने में असमर्थ"},
                {"fr","Impossible de se lier au port local"},
                {"zh-chs","无法绑定到本地端口"},
                {"fi","Ei voi yhdistää paikalliseen porttiin"},
                {"tr","Yerel bağlantı noktasına bağlanamıyor"},
                {"cs","Nelze se vázat na místní port"},
                {"ja","ローカルポートにバインドできません"},
                {"es","No se puede vincular al puerto local"},
                {"pl","Nie można przypisać do portu lokalnego"},
                {"pt","Incapaz de ligar à porta local"},
                {"nl","Kan niet binden aan lokale poort"},
                {"pt-br","Não foi possível conectar à porta local"},
                {"sv","Det gick inte att binda till lokal port"},
                {"da","Kan ikke binde til lokal port"},
                {"ko","로컬 포트에 바인딩 할 수 없습니다."},
                {"it","Impossibile eseguire il binding alla porta locale"},
                {"ru","Невозможно подключиться к локальному порту"}
            }
        },
        {
            "Rename File",
            new Dictionary<string, string>() {
                {"de","Datei umbenennen"},
                {"hi","फाइल का नाम बदलो"},
                {"fr","Renommer le fichier"},
                {"zh-chs","重新命名文件"},
                {"fi","Nimeä tiedosto uudelleen"},
                {"tr","Dosyayı yeniden adlandır"},
                {"cs","Přejmenuj soubor"},
                {"ja","ファイルの名前を変更"},
                {"es","Renombrar Archivo"},
                {"pl","Zmień Nazwę Pliku"},
                {"pt","Renomear arquivo"},
                {"nl","Bestand hernoemen"},
                {"pt-br","Renomear arquivo"},
                {"sv","Döp om fil"},
                {"da","Omdøb fil"},
                {"ko","파일명 변경"},
                {"it","Rinomina il file"},
                {"ru","Переименуйте файл"}
            }
        },
        {
            "Push local clipboard to remote device",
            new Dictionary<string, string>() {
                {"de","Lokale Zwischenablage auf Remote-Gerät übertragen"},
                {"hi","स्थानीय क्लिपबोर्ड को दूरस्थ डिवाइस पर पुश करें"},
                {"fr","Transférer le presse-papiers local vers l'appareil distant"},
                {"zh-chs","将本地剪贴板推送到远程设备"},
                {"fi","Työnnä paikallinen leikepöytä etälaitteeseen"},
                {"tr","Yerel panoyu uzak cihaza itin"},
                {"cs","Přesuňte místní schránku do vzdáleného zařízení"},
                {"ja","ローカルクリップボードをリモートデバイスにプッシュする"},
                {"es","Empuje el portapapeles local al dispositivo remoto"},
                {"pl","Prześlij lokalny schowek do urządzenia zdalnego"},
                {"pt","Envie a área de transferência local para o dispositivo remoto"},
                {"nl","Verplaats lokaal klembord naar extern apparaat"},
                {"pt-br","Envie a área de transferência local para o dispositivo remoto"},
                {"sv","Tryck lokalt urklipp till fjärrenheten"},
                {"da","Skub den lokale udklipsholder til fjernenheden"},
                {"ko","로컬 클립 보드를 원격 장치로 푸시"},
                {"it","Spingi gli appunti locali sul dispositivo remoto"},
                {"ru","Перенести локальный буфер обмена на удаленное устройство"}
            }
        },
        {
            "Overwrite {0} files?",
            new Dictionary<string, string>() {
                {"de","{0} Dateien überschreiben?"},
                {"hi","{0} फ़ाइलें अधिलेखित करें?"},
                {"fr","Écraser {0} fichiers ?"},
                {"zh-chs","覆盖 {0} 个文件？"},
                {"fi","Korvataanko {0} tiedostoa?"},
                {"tr","{0} dosyanın üzerine yazılsın mı?"},
                {"cs","Přepsat {0} souborů?"},
                {"ja","{0}ファイルを上書きしますか？"},
                {"es","¿Sobrescribir {0} archivos?"},
                {"pl","Zastąpić {0} plików?"},
                {"pt","Substituir {0} arquivos?"},
                {"nl","{0} bestanden overschrijven?"},
                {"pt-br","¿Sobrescribir {0} archivos?"},
                {"sv","Skriv över {0} filer?"},
                {"da","Overskriv {0} filer?"},
                {"ko","{0} 파일을 덮어쓰시겠습니까?"},
                {"it","Sovrascrivere {0} file?"},
                {"ru","Перезаписать {0} файлы?"}
            }
        },
        {
            "Two-factor Authentication",
            new Dictionary<string, string>() {
                {"de","Zwei-Faktor-Authentifizierung"},
                {"hi","दो तरीकों से प्रमाणीकरण"},
                {"fr","Authentification à deux facteurs"},
                {"zh-chs","两因素身份验证"},
                {"fi","Kaksivaiheinen todennus"},
                {"tr","İki Faktörlü Kimlik Doğrulama"},
                {"cs","Dvoufaktorové ověřování"},
                {"ja","二要素認証"},
                {"es","Autenticación de Dos Factores"},
                {"pl","Uwierzytelnianie Dwuskładnikowe"},
                {"pt","Autenticação de dois fatores"},
                {"nl","Twee-factor authenticatie"},
                {"pt-br","Autenticação de dois fatores"},
                {"sv","Tvåfaktorsautentisering"},
                {"da","2-faktor godkendelse"},
                {"ko","2 단계 인증"},
                {"it","Autenticazione a due fattori"},
                {"ru","Двухфакторная аутентификация"}
            }
        },
        {
            "label1",
            new Dictionary<string, string>() {
                {"de","Etikett1"},
                {"hi","लेबल1"},
                {"fr","étiquette1"},
                {"zh-chs","标签 1"},
                {"fi","etiketti 1"},
                {"tr","etiket1"},
                {"cs","štítek 1"},
                {"es","etiqueta1"},
                {"pl","etykieta1"},
                {"pt","etiqueta1"},
                {"pt-br","etiqueta1"},
                {"sv","etikett1"},
                {"it","etichetta1"}
            }
        },
        {
            "Email sent",
            new Dictionary<string, string>() {
                {"de","E-Mail gesendet"},
                {"hi","ईमेल भेजा"},
                {"fr","Email envoyé"},
                {"zh-chs","邮件已发送"},
                {"fi","Sähköposti lähetetty"},
                {"tr","E-posta gönderildi"},
                {"cs","Email odeslán"},
                {"ja","送信された電子メール"},
                {"es","Correo electrónico enviado"},
                {"pl","Email wysłano"},
                {"pt","Email enviado"},
                {"nl","E-mail verzonden"},
                {"pt-br","Email enviado"},
                {"sv","Email skickat"},
                {"da","E-mail sendt"},
                {"ko","이메일을 보냈습니다."},
                {"it","Email inviata"},
                {"ru","Письмо отправлено"}
            }
        },
        {
            "Sort by &Name",
            new Dictionary<string, string>() {
                {"de","Nach Name sortieren"},
                {"hi","नाम द्वारा क्रमबद्ध करें"},
                {"fr","Trier par nom"},
                {"zh-chs","按名称分类"},
                {"fi","Lajittele nimen mukaan"},
                {"tr","İsme göre sırala"},
                {"cs","Seřaď dle jména"},
                {"ja","名前順"},
                {"es","Ordenar por nombre"},
                {"pl","Sortuj po &Nazwie"},
                {"pt","Classificar por & nome"},
                {"nl","Sorteer op &Naam"},
                {"pt-br","Classificar por & nome"},
                {"sv","Sortera efter &Namn"},
                {"da","Sortér efter &navn"},
                {"ko","이름으로 분류하다"},
                {"it","Ordina per nome"},
                {"ru","Сортировать по имени"}
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
                {"tr","Anahtar"},
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
                {"pl","Średnio"},
                {"pt","Médio"},
                {"nl","Gemiddeld"},
                {"pt-br","Médio"},
                {"ko","중간"},
                {"it","medio"},
                {"ru","Средний"}
            }
        },
        {
            "Remote Files",
            new Dictionary<string, string>() {
                {"de","Remote-Dateien"},
                {"hi","दूरस्थ फ़ाइलें"},
                {"fr","Fichiers distants"},
                {"zh-chs","远程文件"},
                {"fi","Etätiedostot"},
                {"tr","Uzak Dosyalar"},
                {"cs","Vzdálené soubory"},
                {"ja","リモートファイル"},
                {"es","Archivos Remotos"},
                {"pl","Zdalne Pliki"},
                {"pt","Arquivos Remotos"},
                {"nl","Externe bestanden"},
                {"pt-br","Arquivos Remotos"},
                {"sv","Fjärrfiler"},
                {"da","Fjernfiler"},
                {"ko","원격 파일"},
                {"it","File remoti"},
                {"ru","Удаленные файлы"}
            }
        },
        {
            "RDP Port",
            new Dictionary<string, string>() {
                {"de","RDP-Port"},
                {"hi","आरडीपी पोर्ट"},
                {"fr","Port RDP"},
                {"zh-chs","RDP 端口"},
                {"fi","RDP -portti"},
                {"tr","RDP Bağlantı Noktası"},
                {"cs","Port RDP"},
                {"ja","RDPポート"},
                {"es","Puerto RDP"},
                {"pl","Port RDP"},
                {"pt","Porta RDP"},
                {"nl","RDP poort"},
                {"pt-br","Porta RDP"},
                {"sv","RDP-port"},
                {"da","RDP port"},
                {"ko","RDP 포트"},
                {"it","Porta RDP"},
                {"ru","Порт RDP"}
            }
        },
        {
            "Updating...",
            new Dictionary<string, string>() {
                {"de","Aktualisierung..."},
                {"hi","अपडेट हो रहा है..."},
                {"fr","Mise à jour..."},
                {"zh-chs","正在更新..."},
                {"fi","Päivitetään ..."},
                {"tr","güncelleniyor..."},
                {"cs","Aktualizace ..."},
                {"ja","更新中..."},
                {"es","Actualizando ..."},
                {"pl","Aktualizacja..."},
                {"pt","Atualizando ..."},
                {"nl","Bijwerken..."},
                {"pt-br","Atualizando ..."},
                {"sv","Uppdaterar ..."},
                {"da","Opdaterer..."},
                {"ko","업데이트 중 ..."},
                {"it","In aggiornamento..."},
                {"ru","Обновление ..."}
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
                {"pl","Oczekiwanie na przyznanie dostępu przez użytkownika..."},
                {"pt","Esperando que o usuário conceda acesso ..."},
                {"nl","Wachten op toestemming van de gebruiker..."},
                {"pt-br","Esperando que o usuário conceda acesso ..."},
                {"sv","Väntar på att användaren ska ge åtkomst ..."},
                {"da","Venter på, at brugeren giver adgang..."},
                {"ko","사용자가 액세스 권한을 부여하기를 기다리는 중 ..."},
                {"it","In attesa che l'utente conceda l'accesso... "},
                {"ru","Ожидание предоставления доступа пользователем ..."}
            }
        },
        {
            " MeshCentral Router",
            new Dictionary<string, string>() {
                {"de"," MeshCentral-Router"},
                {"hi"," मेशसेंट्रल राउटर"},
                {"fr"," Routeur MeshCentral"},
                {"zh-chs"," MeshCentral 路由器"},
                {"fi","MeshCentral -reititin"},
                {"tr"," MeshCentral Yönlendirici"},
                {"cs","MeshCentral Router"},
                {"ja","MeshCentralルーター"},
                {"es"," Enrutador MeshCentral"},
                {"pt","Roteador MeshCentral"},
                {"ko"," MeshCentral 라우터"},
                {"it","MeshCentral Router"},
                {"ru","MeshCentral Маршрутизатор"}
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
                {"pl","Zdalny"},
                {"pt","Remoto"},
                {"nl","Extern"},
                {"pt-br","Controle remoto"},
                {"sv","Avlägsen"},
                {"ko","원격"},
                {"it","Remoto"},
                {"ru","Удаленно"}
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
                {"pl","Usuń"},
                {"pt","Remover"},
                {"nl","Verwijderen"},
                {"pt-br","Remover"},
                {"sv","Avlägsna"},
                {"da","Fjern"},
                {"ko","제거"},
                {"it","Rimuovere"},
                {"ru","Удалить"}
            }
        },
        {
            "Failed to start remote terminal session",
            new Dictionary<string, string>() {
                {"de","Fehler beim Starten der Remote-Terminal-Sitzung"},
                {"hi","दूरस्थ टर्मिनल सत्र प्रारंभ करने में विफल"},
                {"fr","Échec du démarrage de la session de terminal distant"},
                {"zh-chs","无法启动远程终端会话"},
                {"fi","Etäpääteistunnon käynnistäminen epäonnistui"},
                {"tr","Uzak terminal oturumu başlatılamadı"},
                {"cs","Spuštění relace vzdáleného terminálu se nezdařilo"},
                {"ja","リモートターミナルセッションの開始に失敗しました"},
                {"es","No se pudo iniciar la sesión de terminal remota"},
                {"pl","Nie udało się uruchomić sesji zdalnego terminala"},
                {"pt","Falha ao iniciar sessão de terminal remoto"},
                {"nl","Kan externe terminalsessie niet starten"},
                {"pt-br","Falha ao iniciar sessão de terminal remoto"},
                {"sv","Det gick inte att starta fjärrterminsessionen"},
                {"da","Kunne ikke starte fjernterminalsession"},
                {"ko","원격 터미널 세션을 시작하지 못했습니다."},
                {"it","Impossibile avviare la sessione del terminale remoto"},
                {"ru","Не удалось запустить сеанс удаленного терминала"}
            }
        },
        {
            "Relay Mapping",
            new Dictionary<string, string>() {
                {"de","Relaiszuordnung"},
                {"hi","रिले मैपिंग"},
                {"fr","Cartographie des relais"},
                {"zh-chs","中继映射"},
                {"fi","Relekartoitus"},
                {"tr","Röle Haritalama"},
                {"cs","Reléové mapování"},
                {"ja","リレーマッピング"},
                {"es","Mapeo de Relés"},
                {"pl","Mapowanie Przekierowania"},
                {"pt","Mapeamento de retransmissão"},
                {"nl","Doorstuurtoewijzing"},
                {"pt-br","Mapeamento de retransmissão"},
                {"sv","Relämappning"},
                {"ko","릴레이 매핑"},
                {"it","Mappatura ritrasmissione"},
                {"ru","Отображение реле"}
            }
        },
        {
            ", {0} users",
            new Dictionary<string, string>() {
                {"de",", {0} Nutzer"},
                {"hi",", {0} उपयोगकर्ता"},
                {"fr",", {0} utilisateurs"},
                {"zh-chs",", {0} 个用户"},
                {"fi",", {0} käyttäjää"},
                {"tr",", {0} kullanıcı"},
                {"cs",", Uživatelé: {0}"},
                {"ja","、{0}ユーザー"},
                {"es",", {0} usuarios"},
                {"pl",", {0} użytkowników"},
                {"pt",", {0} usuários"},
                {"nl",", {0} gebruikers"},
                {"pt-br",", {0} usuários"},
                {"sv",", {0} användare"},
                {"da",", {0} brugere"},
                {"ko",", {0} 명의 사용자"},
                {"it",", {0} utenti"},
                {"ru",", Пользователей: {0}"}
            }
        },
        {
            "SMS sent",
            new Dictionary<string, string>() {
                {"de","SMS gesendet"},
                {"hi","एसएमएस भेजा गया"},
                {"fr","SMS envoyé"},
                {"zh-chs","短信发送"},
                {"fi","SMS lähetetty"},
                {"tr","SMS gönderildi"},
                {"cs","SMS odeslána"},
                {"ja","SMSが送信されました"},
                {"es","SMS enviado"},
                {"pl","SMS wysłany"},
                {"pt","SMS enviado"},
                {"nl","SMS verzonden"},
                {"pt-br","SMS enviado"},
                {"sv","SMS skickat"},
                {"da","SMS sendt"},
                {"ko","SMS 전송"},
                {"it","SMS inviato"},
                {"ru","SMS отправлено"}
            }
        },
        {
            "Remote Desktop Stats",
            new Dictionary<string, string>() {
                {"de","Remotedesktop-Statistiken"},
                {"hi","दूरस्थ डेस्कटॉप आँकड़े"},
                {"fr","Statistiques du bureau à distance"},
                {"zh-chs","远程桌面统计"},
                {"fi","Etätyöpöydän tilastot"},
                {"tr","Uzak Masaüstü İstatistikleri"},
                {"cs","Statistiky vzdálené plochy"},
                {"ja","リモートデスクトップ統計"},
                {"es","Estadísticas de Escritorio Remoto"},
                {"pl","Statystyki Zdalnego Pulpitu"},
                {"pt","Estatísticas da área de trabalho remota"},
                {"nl","Extern bureaublad statistieken"},
                {"pt-br","Estatísticas da área de trabalho remota"},
                {"sv","Statistik för fjärrskrivbord"},
                {"da","Statistik for fjernskrivebord"},
                {"ko","원격 데스크톱 통계"},
                {"it","Statistiche desktop remoto"},
                {"ru","Статистика удаленного рабочего стола"}
            }
        },
        {
            "PuTTY SSH client",
            new Dictionary<string, string>() {
                {"de","PuTTY SSH-Client"},
                {"hi","पुटी एसएसएच क्लाइंट"},
                {"fr","Client SSH PuTTY"},
                {"zh-chs","PuTTY SSH 客户端"},
                {"fi","PuTTY SSH -asiakas"},
                {"tr","PuTTY SSH istemcisi"},
                {"cs","Klient PuTTY SSH"},
                {"ja","PuTTYSSHクライアント"},
                {"es","Cliente PuTTY SSH"},
                {"pl","Klient PuTTY SSH"},
                {"pt","Cliente PuTTY SSH"},
                {"pt-br","Cliente PuTTY SSH"},
                {"sv","PuTTY SSH-klient"},
                {"da","PuTTY SSH-klient"},
                {"ko","PuTTY SSH 클라이언트"},
                {"ru","Клиент PuTTY SSH"}
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
                {"pl","Logowanie"},
                {"pt","Entrar"},
                {"nl","Inloggen"},
                {"pt-br","Conecte-se"},
                {"sv","Logga in"},
                {"ko","로그인"},
                {"ru","Войти"}
            }
        },
        {
            "SSH Username",
            new Dictionary<string, string>() {
                {"de","SSH-Benutzername"},
                {"hi","एसएसएच उपयोगकर्ता नाम"},
                {"fr","Nom d'utilisateur SSH"},
                {"zh-chs","SSH 用户名"},
                {"fi","SSH -käyttäjänimi"},
                {"tr","SSH Kullanıcı Adı"},
                {"cs","Uživatelské jméno SSH"},
                {"ja","SSHユーザー名"},
                {"es","Nombre de usuario SSH"},
                {"pl","Nazwa Użytkownika SSH"},
                {"pt","Nome de usuário SSH"},
                {"nl","SSH gebruikersnaam"},
                {"pt-br","Nome de usuário SSH"},
                {"sv","SSH-användarnamn"},
                {"da","SSH brugernavn"},
                {"ko","SSH 사용자 이름"},
                {"it","Nome utente SSH"},
                {"ru","Имя пользователя SSH"}
            }
        },
        {
            "Set RDP port...",
            new Dictionary<string, string>() {
                {"de","RDP-Port einstellen..."},
                {"hi","आरडीपी पोर्ट सेट करें..."},
                {"fr","Définir le port RDP..."},
                {"zh-chs","设置 RDP 端口..."},
                {"fi","Aseta RDP -portti ..."},
                {"tr","RDP bağlantı noktasını ayarla..."},
                {"cs","Nastavit port RDP ..."},
                {"ja","RDPポートを設定します。"},
                {"es","Establecer puerto RDP ..."},
                {"pl","Ustaw port RDP..."},
                {"pt","Definir porta RDP ..."},
                {"nl","RDP poort instellen..."},
                {"pt-br","Definir porta RDP ..."},
                {"sv","Ställ in RDP-port ..."},
                {"da","Sæt RDP port..."},
                {"ko","RDP 포트 설정 ..."},
                {"it","Imposta porta RDP..."},
                {"ru","Установить порт RDP ..."}
            }
        },
        {
            "Transfer Errors",
            new Dictionary<string, string>() {
                {"de","Übertragungsfehler"},
                {"hi","स्थानांतरण त्रुटियाँ"},
                {"fr","Erreurs de transfert "},
                {"zh-chs","传输错误"},
                {"fi","Siirtovirheet"},
                {"tr","Aktarım Hataları"},
                {"cs","Chyby přenosu"},
                {"ja","転送エラー"},
                {"es","Errores de transferencia"},
                {"pl","Błędy transferu"},
                {"pt","Erros de transferência"},
                {"nl","Overdrachtsfouten"},
                {"pt-br","Errores de transferencia"},
                {"sv","Överföringsfel"},
                {"da","Overførselsfejl"},
                {"ko","전송 오류"},
                {"it","Errori di trasferimento"},
                {"ru","Ошибки передачи"}
            }
        },
        {
            "S&ettings...",
            new Dictionary<string, string>() {
                {"de","Die Einstellungen..."},
                {"hi","समायोजन..."},
                {"fr","Paramètres..."},
                {"zh-chs","设置(&E)..."},
                {"fi","Asetukset..."},
                {"tr","&ayarlar..."},
                {"cs","S & ettings ..."},
                {"ja","設定..."},
                {"es","A&justes..."},
                {"pl","U&stawienia..."},
                {"pt","Definições..."},
                {"nl","Instellingen..."},
                {"pt-br","Configurações..."},
                {"sv","Inställningar..."},
                {"ko","설정 (& E) ..."},
                {"ru","Настройки..."}
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
                {"pl","Hasło"},
                {"pt","Senha"},
                {"nl","Wachtwoord"},
                {"pt-br","Senha"},
                {"sv","Lösenord"},
                {"da","Kodeord"},
                {"ko","암호"},
                {"ru","Пароль"}
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
                {"pl","Nazwa"},
                {"pt","Nome"},
                {"nl","Naam"},
                {"pt-br","Nome"},
                {"sv","Namn"},
                {"da","Navn"},
                {"ko","이름"},
                {"it","Nome"},
                {"ru","Имя"}
            }
        },
        {
            "Add Map...",
            new Dictionary<string, string>() {
                {"de","Karte hinzufügen..."},
                {"hi","नक्शा जोड़ें..."},
                {"fr","Ajouter une carte..."},
                {"zh-chs","添加地图..."},
                {"fi","Lisää kartta ..."},
                {"tr","Harita Ekle..."},
                {"cs","Přidat mapu ..."},
                {"ja","地図を追加..."},
                {"es","Agregar mapa ..."},
                {"pl","Dodaj Mapę..."},
                {"pt","Adicionar mapa ..."},
                {"nl","Kaart toevoegen..."},
                {"pt-br","Adicionar mapa ..."},
                {"sv","Lägg till karta ..."},
                {"da","Tilføj kort..."},
                {"ko","지도 추가 ..."},
                {"it","Aggiungi mappa..."},
                {"ru","Добавить карту ..."}
            }
        },
        {
            "Outgoing Bytes",
            new Dictionary<string, string>() {
                {"de","Ausgehende Bytes"},
                {"hi","आउटगोइंग बाइट्स"},
                {"fr","Octets sortants"},
                {"zh-chs","传出字节"},
                {"fi","Lähtevät tavut"},
                {"tr","Giden Bayt"},
                {"cs","Odchozí bajty"},
                {"ja","発信バイト"},
                {"es","Bytes Salientes"},
                {"pl","Bajty Wychodzące"},
                {"pt","Bytes de saída"},
                {"nl","Uitgaande Bytes"},
                {"pt-br","Bytes de saída"},
                {"sv","Utgående byte"},
                {"da","Udgående bytes"},
                {"ko","나가는 바이트"},
                {"it","Byte in uscita"},
                {"ru","Исходящие байты"}
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
                {"pl","Zamień Przyciski Myszy"},
                {"pt","Botões de troca do mouse"},
                {"nl","Wissel muisknoppen"},
                {"pt-br","Trocar botões do mouse"},
                {"sv","Byt musknappar"},
                {"da","Byt museknapper"},
                {"ko","마우스 버튼 교체"},
                {"it","Scambia i pulsanti del mouse"},
                {"ru","Поменять местами кнопки мыши"}
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
            "Log out",
            new Dictionary<string, string>() {
                {"de","Ausloggen"},
                {"hi","लॉग आउट"},
                {"fr","Se déconnecter"},
                {"zh-chs","登出"},
                {"fi","Kirjautua ulos"},
                {"tr","Çıkış Yap"},
                {"cs","Odhlásit se"},
                {"ja","ログアウト"},
                {"es","Cerrar sesión"},
                {"pl","Wylogowanie"},
                {"pt","Sair"},
                {"nl","Uitloggen"},
                {"pt-br","Sair"},
                {"sv","Logga ut"},
                {"da","Log ud"},
                {"ko","로그 아웃"},
                {"it","Disconnettersi"},
                {"ru","Выйти"}
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
                {"pl","Liczba klatek na sekundę"},
                {"pt","Taxa de quadros"},
                {"nl","Frameverhouding"},
                {"pt-br","Taxa de quadros"},
                {"sv","Bildhastighet"},
                {"da","Billedhastighed"},
                {"ko","프레임 속도"},
                {"it","Frequenza dei fotogrammi"},
                {"ru","Частота кадров"}
            }
        },
        {
            "Tunnelling Data",
            new Dictionary<string, string>() {
                {"de","Tunneling-Daten"},
                {"hi","टनलिंग डेटा"},
                {"fr","Données de tunneling"},
                {"zh-chs","隧道数据"},
                {"fi","Tunnelin tiedot"},
                {"tr","Tünel Açma Verileri"},
                {"cs","Data o tunelování"},
                {"ja","トンネリングデータ"},
                {"es","Datos de Tunelización"},
                {"pl","Dane tunelowania"},
                {"pt","Dados de tunelamento"},
                {"nl","Gegevens tunnelen"},
                {"pt-br","Dados de tunelamento"},
                {"sv","Tunneldata"},
                {"da","Tunneldata"},
                {"ko","터널링 데이터"},
                {"it","Dati di tunneling"},
                {"ru","Данные туннелирования"}
            }
        },
        {
            "Unable to connect",
            new Dictionary<string, string>() {
                {"de","Verbindung konnte nicht hergestellt werden"},
                {"hi","कनेक्ट करने में असमर्थ"},
                {"fr","Impossible de se connecter"},
                {"zh-chs","无法连接"},
                {"fi","Yhteyden muodostaminen ei onnistu"},
                {"tr","Bağlanılamıyor"},
                {"cs","Nelze se připojit"},
                {"ja","つなげられない"},
                {"es","No se puede conectar"},
                {"pl","Nie można się połączyć"},
                {"pt","Incapaz de conectar"},
                {"nl","Niet in staat te verbinden"},
                {"pt-br","Não foi possível conectar "},
                {"sv","Kan inte ansluta"},
                {"da","Kan ikke oprette forbindelse"},
                {"ko","연결할 수 없습니다"},
                {"it","Impossibile connetersi"},
                {"ru","Невозможно подключиться"}
            }
        },
        {
            "Recursive Delete",
            new Dictionary<string, string>() {
                {"de","Rekursives Löschen"},
                {"hi","पुनरावर्ती हटाएं"},
                {"fr","Suppression récursive"},
                {"zh-chs","递归删除"},
                {"fi","Rekursiivinen poisto"},
                {"tr","Özyinelemeli Silme"},
                {"cs","Rekurzivní odstranění"},
                {"ja","再帰的削除"},
                {"es","Eliminación Recursiva"},
                {"pl","Usuwanie Rekursywne"},
                {"pt","Exclusão recursiva"},
                {"nl","Recursief verwijderen"},
                {"pt-br","Exclusão recursiva"},
                {"sv","Rekursivt Radera"},
                {"da","Rekursiv Sletning"},
                {"ko","재귀 삭제"},
                {"it","Cancellazione ricorsiva"},
                {"ru","Рекурсивное удаление"}
            }
        },
        {
            "Port {0} to port {1}",
            new Dictionary<string, string>() {
                {"de","Port {0} zu Port {1}"},
                {"hi","पोर्ट {0} से पोर्ट {1}"},
                {"fr","Port {0} vers port {1}"},
                {"zh-chs","端口 {0} 到端口 {1}"},
                {"fi","Portti {0} porttiin {1}"},
                {"tr","{0} numaralı bağlantı noktasını {1} numaralı bağlantı noktasına"},
                {"cs","Z portu {0} na port {1}"},
                {"ja","ポート{0}からポート{1}"},
                {"es","Puerto {0} al puerto {1}"},
                {"pl","Port {0} do portu {1}"},
                {"pt","Porta {0} para porta {1}"},
                {"nl","Poort {0} naar poort {1}"},
                {"pt-br","Porta {0} para porta {1}"},
                {"sv","Porta {0} till port {1}"},
                {"da","Port {0} til port {1}"},
                {"ko","포트 {0}에서 포트 {1}로"},
                {"it","Porta {0} a porta {1}"},
                {"ru","Порт {0} в порт {1}"}
            }
        },
        {
            "&Info...",
            new Dictionary<string, string>() {
                {"hi","जानकारी..."},
                {"zh-chs","＆信息..."},
                {"fi","&Tiedot..."},
                {"tr","&Bilgi..."},
                {"cs","& Informace ..."},
                {"ja","＆情報..."},
                {"es","&Info ..."},
                {"pt","& Info ..."},
                {"pt-br","&Info ..."},
                {"ko","정보 ..."},
                {"ru","&Инфо..."}
            }
        },
        {
            "Automatically Send Clipboard",
            new Dictionary<string, string>() {
                {"de","Zwischenablage automatisch senden"},
                {"hi","स्वचालित रूप से क्लिपबोर्ड भेजें"},
                {"fr","Envoyer automatiquement dans le presse-papier"},
                {"zh-chs","自动发送剪贴板"},
                {"fi","Lähetä leikepöytä automaattisesti"},
                {"tr","Panoyu Otomatik Olarak Gönder"},
                {"cs","Automaticky odeslat schránku"},
                {"ja","クリップボードを自動的に送信する"},
                {"es","Enviar portapapeles automáticamente"},
                {"pl","Automatycznie wyślij schowek"},
                {"pt","Enviar área de transferência automaticamente"},
                {"nl","Automatisch klembord verzenden"},
                {"pt-br","Enviar Área de Transferência Automaticamente"},
                {"sv","Skicka urklipp automatiskt"},
                {"da","Automatisk sende udklipsholder"},
                {"ko","자동으로 클립보드 보내기"},
                {"it","Invia automaticamente appunti"},
                {"ru","Автоматически отправлять буфер обмена"}
            }
        },
        {
            "&Delete",
            new Dictionary<string, string>() {
                {"de","&Löschen"},
                {"hi","&हटाएं"},
                {"fr","&Effacer"},
                {"zh-chs","＆删除"},
                {"fi","&Poistaa"},
                {"tr","&Silmek"},
                {"cs","&Vymazat"},
                {"ja","＆消去"},
                {"es","&Borrar"},
                {"pl","&Usuń"},
                {"pt","&Excluir"},
                {"nl","&Verwijderen"},
                {"pt-br","&Excluir"},
                {"sv","&Radera"},
                {"ko","&지우다"},
                {"it","&Elimina"},
                {"ru","&Удалить"}
            }
        },
        {
            "Open...",
            new Dictionary<string, string>() {
                {"de","Öffnen..."},
                {"hi","खुला हुआ..."},
                {"fr","Ouvert..."},
                {"zh-chs","打开..."},
                {"fi","Avata..."},
                {"tr","Açık..."},
                {"cs","Otevřeno..."},
                {"ja","開ける..."},
                {"es","Abierto..."},
                {"pl","Otwórz..."},
                {"pt","Abrir..."},
                {"pt-br","Abrir..."},
                {"sv","Öppna..."},
                {"da","Åben..."},
                {"ko","열다..."},
                {"it","Aperto..."},
                {"ru","Открытым..."}
            }
        },
        {
            "Toggle remote desktop connection",
            new Dictionary<string, string>() {
                {"de","Remotedesktopverbindung umschalten"},
                {"hi","दूरस्थ डेस्कटॉप कनेक्शन टॉगल करें"},
                {"fr","Basculer la connexion au bureau à distance"},
                {"zh-chs","切换远程桌面连接"},
                {"fi","Vaihda etätyöpöytäyhteys"},
                {"tr","Uzak masaüstü bağlantısını değiştir"},
                {"cs","Přepnout připojení ke vzdálené ploše"},
                {"ja","リモートデスクトップ接続を切り替えます"},
                {"es","Alternar la conexión de escritorio remoto"},
                {"pl","Przełącz zdalnego połączenia pulpitu"},
                {"pt","Alternar conexão de área de trabalho remota"},
                {"nl","Verbinding met extern bureaublad wisselen"},
                {"pt-br","Alternar conexão de área de trabalho remota"},
                {"sv","Växla fjärrskrivbordsanslutning"},
                {"da","Skift fjernskrivebordsforbindelse"},
                {"ko","원격 데스크톱 연결 전환"},
                {"it","Attiva/disattiva connessione desktop remoto"},
                {"ru","Переключить подключение к удаленному рабочему столу"}
            }
        },
        {
            "Starting...",
            new Dictionary<string, string>() {
                {"de","Beginnend..."},
                {"hi","शुरुआत..."},
                {"fr","Départ..."},
                {"zh-chs","开始..."},
                {"fi","Aloitetaan ..."},
                {"tr","Başlangıç..."},
                {"cs","Začínající..."},
                {"ja","起動..."},
                {"es","A partir de..."},
                {"pl","Start..."},
                {"pt","Iniciando..."},
                {"nl","Starten..."},
                {"pt-br","Iniciando..."},
                {"sv","Startande..."},
                {"da","Starter..."},
                {"ko","시작하는 중 ..."},
                {"it","In Avvio"},
                {"ru","Пуск ..."}
            }
        },
        {
            "MeshCentral Router allows mapping of TCP and UDP ports on this computer to any computer in your MeshCentral server account. Start by logging into your account.",
            new Dictionary<string, string>() {
                {"de","MeshCentral Router ermöglicht die Zuordnung von TCP- und UDP-Ports auf diesem Computer zu jedem Computer in Ihrem MeshCentral-Serverkonto. Melden Sie sich zunächst bei Ihrem Konto an."},
                {"hi","MeshCentral राउटर इस कंप्यूटर पर आपके MeshCentral सर्वर खाते के किसी भी कंप्यूटर पर TCP और UDP पोर्ट की मैपिंग की अनुमति देता है। अपने खाते में लॉग इन करके प्रारंभ करें।"},
                {"fr","Le routeur MeshCentral permet de mapper les ports TCP et UDP de cet ordinateur sur n'importe quel ordinateur de votre compte de serveur MeshCentral. Commencez par vous connecter à votre compte."},
                {"zh-chs","MeshCentral 路由器允许将此计算机上的 TCP 和 UDP 端口映射到您的 MeshCentral 服务器帐户中的任何计算机。首先登录您的帐户。"},
                {"fi","MeshCentral Router mahdollistaa tämän tietokoneen TCP- ja UDP -porttien yhdistämisen mihin tahansa MeshCentral -palvelintilisi tietokoneeseen. Aloita kirjautumalla tilillesi."},
                {"tr","MeshCentral Router, bu bilgisayardaki TCP ve UDP bağlantı noktalarının MeshCentral sunucu hesabınızdaki herhangi bir bilgisayara eşlenmesine izin verir. Hesabınıza giriş yaparak başlayın."},
                {"cs","MeshCentral Router umožňuje mapování portů TCP a UDP na tomto počítači na jakýkoli počítač ve vašem účtu serveru MeshCentral. Začněte přihlášením ke svému účtu."},
                {"ja","MeshCentralルーターを使用すると、このコンピューターのTCPポートとUDPポートをMeshCentralサーバーアカウントの任意のコンピューターにマッピングできます。アカウントにログインすることから始めます。"},
                {"es","MeshCentral Router permite la asignación de puertos TCP y UDP en esta computadora a cualquier computadora en tu cuenta de servidor MeshCentral. Empieza por iniciar sesión en tu cuenta."},
                {"pl","MeshCentral Router pozwala na mapowanie portów TCP i UDP na tym komputerze do dowolnego komputera w twoim koncie serwera MeshCentral. Zacznij od zalogowania się na swoje konto."},
                {"pt","O roteador MeshCentral permite o mapeamento das portas TCP e UDP neste computador para qualquer computador em sua conta do servidor MeshCentral. Comece fazendo login em sua conta."},
                {"nl","Met MeshCentral Router kunnen TCP en UDP poorten op deze computer worden toegewezen aan elke computer in uw MeshCentral-serveraccount. Begin door in te loggen op uw account."},
                {"pt-br","O MeshCentral Router permite o mapeamento das portas TCP e UDP neste computador para qualquer computador em sua conta do servidor MeshCentral. Comece fazendo login em sua conta."},
                {"sv","MeshCentral Router tillåter mappning av TCP- och UDP-portar på den här datorn till vilken dator som helst i ditt MeshCentral-serverkonto. Börja med att logga in på ditt konto."},
                {"da","MeshCentral Router tillader kortlægning af TCP- og UDP-porte på denne computer til enhver computer på din MeshCentral-serverkonto. Start med at logge ind på din konto."},
                {"ko","MeshCentral 라우터를 사용하면이 컴퓨터의 TCP 및 UDP 포트를 MeshCentral 서버 계정의 모든 컴퓨터에 매핑 할 수 있습니다. 계정에 로그인하여 시작하십시오."},
                {"it","MeshCentral Router consente la mappatura delle porte TCP e UDP su questo computer su qualsiasi computer nell'account del server MeshCentral. Inizia accedendo al tuo account."},
                {"ru","Маршрутизатор MeshCentral позволяет сопоставить порты TCP и UDP на этом компьютере с любым компьютером в вашей учетной записи сервера MeshCentral. Начните с входа в свою учетную запись."}
            }
        },
        {
            "ServerName",
            new Dictionary<string, string>() {
                {"de","Servername"},
                {"hi","सर्वर का नाम"},
                {"fr","Nom du serveur"},
                {"zh-chs","服务器名称"},
                {"fi","Palvelimen nimi"},
                {"tr","Sunucu adı"},
                {"cs","Název serveru"},
                {"ja","サーバーの名前"},
                {"es","Nombre del Servidor"},
                {"pl","Nazwa serwera"},
                {"pt","Nome do servidor"},
                {"nl","Servernaam"},
                {"pt-br","Nome do servidor"},
                {"sv","Server namn"},
                {"ko","서버 이름"},
                {"it","Nome del server"},
                {"ru","Название сервера"}
            }
        },
        {
            "Sort by G&roup",
            new Dictionary<string, string>() {
                {"de","Nach Gruppe sortieren"},
                {"hi","समूह के आधार पर छाँटें"},
                {"fr","Trier par groupe"},
                {"zh-chs","按组(&O) 排序"},
                {"fi","Lajittele G & roup"},
                {"tr","G&grubuna göre sırala"},
                {"cs","Seřadit podle G & roup"},
                {"ja","G＆roupで並べ替え"},
                {"es","Ordenar por grupo y grupo"},
                {"pl","Sortuj po G&rupie"},
                {"pt","Classificar por G & Rupo"},
                {"nl","Sorteer op G&roep"},
                {"pt-br","Classificar por G & Rupo"},
                {"sv","Sortera efter G & roup"},
                {"da","Sortér efter G&roup"},
                {"ko","그룹 별 정렬 (& R)"},
                {"it","Ordina per gruppo"},
                {"ru","Сортировать по группе"}
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
                {"pl","Jakość"},
                {"pt","Qualidade"},
                {"nl","Kwaliteit"},
                {"pt-br","Qualidade"},
                {"sv","Kvalitet"},
                {"da","Kvalitet"},
                {"ko","품질"},
                {"it","Qualità"},
                {"ru","Качество"}
            }
        },
        {
            "Remember this certificate",
            new Dictionary<string, string>() {
                {"de","Merken Sie sich dieses Zertifikat"},
                {"hi","यह प्रमाणपत्र याद रखें"},
                {"fr","Rappelez-vous ce certificat"},
                {"zh-chs","记住这个证书"},
                {"fi","Muista tämä todistus"},
                {"tr","Bu sertifikayı hatırla"},
                {"cs","Zapamatujte si tento certifikát"},
                {"ja","この証明書を覚えておいてください"},
                {"es","Recuerda este certificado"},
                {"pl","Zapamiętaj ten certyfikat"},
                {"pt","Lembre-se deste certificado"},
                {"nl","Onthoud dit certificaat"},
                {"pt-br","Lembrar deste certificado"},
                {"sv","Kom ihåg detta certifikat"},
                {"da","Husk dette certifikat"},
                {"ko","이 인증서 기억"},
                {"it","Ricorda questo certificato"},
                {"ru","Запомни этот сертификат"}
            }
        },
        {
            "Incoming Compression",
            new Dictionary<string, string>() {
                {"de","Eingehende Kompression"},
                {"hi","आने वाली संपीड़न"},
                {"fr","Compression entrante"},
                {"zh-chs","传入压缩"},
                {"fi","Saapuva pakkaus"},
                {"tr","Gelen Sıkıştırma"},
                {"cs","Příchozí komprese"},
                {"ja","着信圧縮"},
                {"es","Compresión entrante"},
                {"pl","Kompresja Wejściowa"},
                {"pt","Compressão de entrada"},
                {"nl","Inkomende compressie"},
                {"pt-br","Compressão de entrada"},
                {"sv","Inkommande kompression"},
                {"da","Indkommende kompression"},
                {"ko","들어오는 압축"},
                {"it","Compressione in entrata"},
                {"ru","Входящее сжатие"}
            }
        },
        {
            "Languages",
            new Dictionary<string, string>() {
                {"de","Sprachen"},
                {"hi","बोली"},
                {"fr","Langues"},
                {"zh-chs","语言"},
                {"fi","Kieli (kielet"},
                {"tr","Diller"},
                {"cs","Jazyky"},
                {"ja","言語"},
                {"es","Idiomas"},
                {"pl","Języki"},
                {"pt","línguas"},
                {"nl","Talen"},
                {"pt-br","Idiomas"},
                {"sv","språk"},
                {"da","Sprog"},
                {"ko","언어"},
                {"it","Linguaggi"},
                {"ru","Языки"}
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
                {"it","Router MeshCentral"},
                {"ru","MeshCentral Router "}
            }
        },
        {
            "&Open...",
            new Dictionary<string, string>() {
                {"de","&Öffnen..."},
                {"hi","&खुला हुआ..."},
                {"fr","&Ouvert..."},
                {"zh-chs","＆打开..."},
                {"fi","&Avata..."},
                {"tr","&Açık..."},
                {"cs","&Otevřeno..."},
                {"ja","＆開ける..."},
                {"es","&Abierto..."},
                {"pl","&Otwórz..."},
                {"pt","&Abrir..."},
                {"pt-br","&Abrir..."},
                {"sv","&Öppna..."},
                {"ko","&열다..."},
                {"it","&Apri..."},
                {"ru","&Открыть"}
            }
        },
        {
            "{0} Byte",
            new Dictionary<string, string>() {
                {"hi","{0} बाइट"},
                {"fr","{0} octet"},
                {"zh-chs","{0} 字节"},
                {"fi","{0} Tavu"},
                {"tr","{0} Bayt"},
                {"ja","{0}バイト"},
                {"pl","{0} Bajt"},
                {"ko","{0} 바이트"},
                {"ru","{0} байт"}
            }
        },
        {
            "statusStrip1",
            new Dictionary<string, string>() {
                {"hi","स्थिति पट्टी1"},
                {"zh-chs","状态条1"},
                {"tr","durumStrip1"},
                {"it","statoStrip1"}
            }
        },
        {
            "No Port Mappings\r\n\r\nClick \"Add\" to get started.",
            new Dictionary<string, string>() {
                {"de","Keine Portzuordnungen\r\n\r\nKlicken Sie auf \"Hinzufügen\", um zu beginnen."},
                {"hi","कोई पोर्ट मैपिंग नहीं\r\n\r\nआरंभ करने के लिए \"जोड़ें\" पर क्लिक करें।"},
                {"fr","Aucun mappage de port\r\n\r\nCliquez sur \"Ajouter\" pour commencer."},
                {"zh-chs","无端口映射\r\n\r\n单击“添加”开始。"},
                {"fi","Ei satamakartoituksia\r\n\r\nAloita napsauttamalla \"Lisää\"."},
                {"tr","Bağlantı Noktası Eşleme Yok\r\n\r\nBaşlamak için \"Ekle\"yi tıklayın."},
                {"cs","Žádné mapování portů\r\n\r\nZačněte kliknutím na „Přidat“."},
                {"ja","ポートマッピングなし\r\n\r\n「追加」をクリックして開始します。"},
                {"es","Sin Asignaciones de Puertos\r\n\r\nHaz clic en \"Agregar\" para comenzar."},
                {"pl","Brak Mapowań Portów\n\nKliknij \"Dodaj\", aby rozpocząć."},
                {"pt","Sem mapeamentos de portas\r\n\r\nClique em \"Adicionar\" para começar."},
                {"nl","Geen poorttoewijzingen\r\n\r\nKlik op \"Toevoegen\" om te beginnen."},
                {"pt-br","Sem mapeamentos de portas\r\n\r\nClique em \"Adicionar\" para começar."},
                {"sv","Inga portkartor\r\n\r\nKlicka på \"Lägg till\" för att komma igång."},
                {"da","Ingen portmapninger\n\nKlik på \"Tilføj\" for at komme i gang."},
                {"ko","포트 매핑 없음\r\n\r\n시작하려면 \"추가\"를 클릭하십시오."},
                {"it","Nessuna mappatura delle porte.\r\n\r\nFai clic su \"Aggiungi\" per iniziare."},
                {"ru","Нет сопоставления портов\r\n\r\nНажмите «Добавить», чтобы начать."}
            }
        },
        {
            "Server",
            new Dictionary<string, string>() {
                {"hi","सर्वर"},
                {"fr","Serveur"},
                {"zh-chs","服务器"},
                {"fi","Palvelin"},
                {"tr","sunucu"},
                {"ja","サーバ"},
                {"es","Servidor"},
                {"pl","Serwer"},
                {"pt","Servidor"},
                {"pt-br","Servidor"},
                {"ko","서버"},
                {"ru","Сервер"}
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
                {"pl","Połączono"},
                {"pt","Conectado"},
                {"nl","Verbonden"},
                {"pt-br","Conectado"},
                {"sv","Ansluten"},
                {"da","Forbundet"},
                {"ko","연결됨"},
                {"it","Collegato"},
                {"ru","Подключено"}
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
                {"pl","Zamknij"},
                {"pt","Fechar"},
                {"nl","Sluiten"},
                {"pt-br","Fechar"},
                {"sv","Stäng"},
                {"da","Luk"},
                {"ko","닫기"},
                {"it","Chiudere"},
                {"ru","Закрыть"}
            }
        },
        {
            "Local - {0}",
            new Dictionary<string, string>() {
                {"de","Lokal - {0}"},
                {"hi","स्थानीय - {0}"},
                {"fr","Locale - {0}"},
                {"zh-chs","本地 - {0}"},
                {"fi","Paikallinen - {0}"},
                {"tr","Yerel - {0}"},
                {"cs","Místní - {0}"},
                {"ja","ローカル-{0}"},
                {"es","Local: {0}"},
                {"pl","Lokalny - {0}"},
                {"nl","Lokaal - {0}"},
                {"sv","Lokalt - {0}"},
                {"da","Lokalt - {0}"},
                {"ko","지역-{0}"},
                {"it","Locale - {0}"},
                {"ru","Местный - {0}"}
            }
        },
        {
            "No Search Results",
            new Dictionary<string, string>() {
                {"de","keine Suchergebnisse"},
                {"hi","खोजने पर कोई परिणाम नहीं मिला"},
                {"fr","aucun résultat trouvé"},
                {"zh-chs","没有搜索结果"},
                {"fi","Ei hakutuloksia"},
                {"tr","arama sonucu bulunamadı"},
                {"cs","Žádné výsledky vyhledávání"},
                {"ja","検索結果がありません"},
                {"es","Sin Resultados de Búsqueda"},
                {"pl","Brak Wyników Szukania"},
                {"pt","Sem resultados de pesquisa"},
                {"nl","geen resultaten gevonden"},
                {"pt-br","Sem resultados de pesquisa"},
                {"sv","inga sökresultat"},
                {"da","Ingen søgeresultater"},
                {"ko","검색 결과 없음"},
                {"it","nessun risultato trovato"},
                {"ru","Нет Результатов Поиска"}
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
                {"pl","Ustawienia"},
                {"pt","Configurações"},
                {"nl","Instellingen"},
                {"pt-br","Configurações"},
                {"sv","inställningar"},
                {"da","Indstillinger"},
                {"ko","설정"},
                {"it","impostazioni"},
                {"ru","Настройки"}
            }
        },
        {
            "No tools allowed",
            new Dictionary<string, string>() {
                {"de","Kein Werkzeug erlaubt"},
                {"hi","किसी उपकरण की अनुमति नहीं है"},
                {"fr","Aucun utilitaire autorisé"},
                {"zh-chs","不允许使用工具"},
                {"fi","Ei työkaluja"},
                {"tr","Hiçbir alete izin verilmez"},
                {"cs","Nejsou povoleny žádné nástroje"},
                {"ja","ツールは許可されていません"},
                {"es","No se permiten herramientas"},
                {"pl","Żadne narzędzia nie są dozwolone"},
                {"pt","Nenhuma ferramenta permitida"},
                {"nl","Geen gereedschap toegestaan"},
                {"pt-br","Nenhuma ferramenta permitida"},
                {"sv","Inga verktyg tillåtna"},
                {"da","Ingen værktøj tilladt"},
                {"ko","도구가 허용되지 않음"},
                {"it","Nessuno strumento consentito"},
                {"ru","Инструменты не разрешены"}
            }
        },
        {
            "{0} minutes left",
            new Dictionary<string, string>() {
                {"de","noch {0} Minuten"},
                {"hi","{0} मिनट शेष"},
                {"fr","{0} minutes restantes"},
                {"zh-chs","还剩 {0} 分钟"},
                {"fi","{0} minuuttia jäljellä"},
                {"tr","{0} dakika kaldı"},
                {"cs","Zbývají {0} minuty"},
                {"ja","残り{0}分"},
                {"es","Quedan {0} minutos"},
                {"pl","Pozostało {0} minut"},
                {"pt","Faltam {0} minutos"},
                {"nl","{0} minuten resterend"},
                {"pt-br","Quedan {0} minutos"},
                {"sv","{0} minuter kvar"},
                {"da","{0} minutter tilbage"},
                {"ko","{0}분 남음"},
                {"it","{0} minuti rimasti"},
                {"ru","Осталось {0} минут"}
            }
        },
        {
            "Pull clipboard from remote device",
            new Dictionary<string, string>() {
                {"de","Zwischenablage von Remote-Gerät ziehen"},
                {"hi","दूरस्थ डिवाइस से क्लिपबोर्ड खींचे"},
                {"fr","Extraire le presse-papiers de l'appareil distant"},
                {"zh-chs","从远程设备拉剪贴板"},
                {"fi","Vedä leikepöytä etälaitteesta"},
                {"tr","Panoyu uzak cihazdan çekin"},
                {"cs","Vytáhněte schránku ze vzdáleného zařízení"},
                {"ja","リモートデバイスからクリップボードを引き出す"},
                {"es","Extraer el Portapapeles del dispositivo remoto"},
                {"pl","Wyciągnij schowek z urządzenia zdalnego"},
                {"pt","Puxe a área de transferência do dispositivo remoto"},
                {"nl","Trek het klembord van het externe apparaat"},
                {"pt-br","Puxe a área de transferência do dispositivo remoto"},
                {"sv","Dra urklipp från fjärrenheten"},
                {"da","Hent udklipsholderen fra fjernenheden"},
                {"ko","원격 장치에서 클립 보드 가져 오기"},
                {"it","Estrai gli appunti dal dispositivo remoto"},
                {"ru","Извлечь буфер обмена с удаленного устройства"}
            }
        },
        {
            "Error uploading file: {0}",
            new Dictionary<string, string>() {
                {"de","Fehler beim Hochladen der Datei: {0}"},
                {"hi","फ़ाइल अपलोड करने में त्रुटि: {0}"},
                {"fr","Erreur lors du chargement du fichier : {0}"},
                {"zh-chs","上传文件时出错：{0}"},
                {"fi","Virhe tiedoston lataamisessa: {0}"},
                {"tr","Dosya yüklenirken hata oluştu: {0}"},
                {"cs","Chyba při nahrávání souboru: {0}"},
                {"ja","ファイルのアップロード中にエラーが発生しました：{0}"},
                {"es","Error al cargar el archivo: {0}"},
                {"pl","Błąd podczas przesyłania pliku: {0}"},
                {"pt","Erro ao enviar arquivo: {0}"},
                {"nl","Fout bij uploaden van bestand: {0}"},
                {"pt-br","Erro ao carregar arquivo: {0}"},
                {"sv","Fel vid överföring av fil: {0}"},
                {"da","Fejl ved upload af fil: {0}"},
                {"ko","파일 업로드 오류: {0}"},
                {"it","Errore durante il caricamento del file: {0}"},
                {"ru","Ошибка при загрузке файла: {0}"}
            }
        },
        {
            "Add &Relay Map...",
            new Dictionary<string, string>() {
                {"de","&Relay-Karte hinzufügen..."},
                {"hi","मानचित्र &रिले जोड़ें..."},
                {"fr","Ajouter une carte de &relais..."},
                {"zh-chs","添加中继地图 (&R)..."},
                {"fi","Lisää ja välitä kartta ..."},
                {"tr","Haritayı &Geçerek Ekle..."},
                {"cs","Přidat a znovu odeslat mapu ..."},
                {"ja","＆リレーマップを追加..."},
                {"es","Agregar y retransmitir mapa ..."},
                {"pl","Dodaj Mapę $Przekierowania..."},
                {"pt","Adicionar & retransmitir mapa ..."},
                {"nl","&Relay toewijzing toevoegen..."},
                {"pt-br","Adicionar & retransmitir mapa ..."},
                {"sv","Lägg till & vidarebefordra karta ..."},
                {"da","Tilføj &Relay Map..."},
                {"ko","릴레이 맵 추가 ..."},
                {"it","Aggiungi &mappa di inoltro..."},
                {"ru","Добавить & карту реле ..."}
            }
        },
        {
            "Email verification required",
            new Dictionary<string, string>() {
                {"de","E-Mail-Verifizierung erforderlich"},
                {"hi","ईमेल सत्यापन आवश्यक"},
                {"fr","Vérification de l'e-mail requise"},
                {"zh-chs","需要电子邮件验证"},
                {"fi","Sähköpostin vahvistus vaaditaan"},
                {"tr","E-posta doğrulaması gerekli"},
                {"cs","Je vyžadováno ověření e -mailem"},
                {"ja","メールによる確認が必要"},
                {"es","Se requiere verificación de correo electrónico"},
                {"pl","Wymagana weryfikacja email"},
                {"pt","Verificação de e-mail necessária"},
                {"nl","E-mailverificatie vereist"},
                {"pt-br","Verificação de e-mail necessária"},
                {"sv","Verifiering av e-post krävs"},
                {"da","E-mail bekræftelse påkrævet"},
                {"ko","이메일 확인 필요"},
                {"it","Verifica e-mail richiesta"},
                {"ru","Требуется подтверждение по электронной почте"}
            }
        },
        {
            "&Rename",
            new Dictionary<string, string>() {
                {"de","&Umbenennen"},
                {"hi","&नाम बदलें"},
                {"fr","&Renommer"},
                {"zh-chs","＆改名"},
                {"fi","&Nimeä uudelleen"},
                {"tr","&Yeniden isimlendirmek"},
                {"cs","&Přejmenovat"},
                {"ja","＆名前の変更"},
                {"es","&Renombrar"},
                {"pl","&Zmień nazwę"},
                {"pt","& Renomear"},
                {"nl","&Hernoemen"},
                {"pt-br","&Renomear"},
                {"sv","&Döp om"},
                {"ko","이름 바꾸기"},
                {"it","&Rinominare"},
                {"ru","&Переименовать"}
            }
        },
        {
            "Unable to write file: {0}",
            new Dictionary<string, string>() {
                {"de","Datei kann nicht geschrieben werden: {0}"},
                {"hi","फ़ाइल लिखने में असमर्थ: {0}"},
                {"fr","Impossible d'écrire dans le fichier : {0}"},
                {"zh-chs","无法写入文件：{0}"},
                {"fi","Tiedoston kirjoittaminen ei onnistu: {0}"},
                {"tr","Dosya yazılamıyor: {0}"},
                {"cs","Nelze zapsat soubor: {0}"},
                {"ja","ファイルを書き込めません：{0}"},
                {"es","No se puede escribir el archivo: {0}"},
                {"pl","Nie można zapisać pliku: {0}"},
                {"pt","Não foi possível gravar o arquivo: {0}"},
                {"nl","Kan bestand: {0} niet schrijven"},
                {"pt-br","No se puede escribir el archivo: {0}"},
                {"sv","Det gick inte att skriva filen: {0}"},
                {"da","Kan ikke skrive fil: {0}"},
                {"ko","파일을 쓸 수 없음: {0}"},
                {"it","Impossibile scrivere il file: {0}"},
                {"ru","Невозможно записать файл: {0}"}
            }
        },
        {
            "Application Name",
            new Dictionary<string, string>() {
                {"de","Anwendungsname"},
                {"hi","आवेदन का नाम"},
                {"fr","Nom de l'application"},
                {"zh-chs","应用名称"},
                {"fi","sovelluksen nimi"},
                {"tr","Uygulama Adı"},
                {"cs","název aplikace"},
                {"ja","アプリケーション名"},
                {"es","Nombre de la aplicación"},
                {"pl","Nazwa Aplikacji"},
                {"pt","Nome da Aplicação"},
                {"nl","Naam van de toepassing"},
                {"pt-br","Nome do Aplicativo"},
                {"sv","applikationsnamn"},
                {"da","applikationsnavn"},
                {"ko","응용 프로그램 이름"},
                {"it","Nome dell'applicazione"},
                {"ru","Имя приложения"}
            }
        },
        {
            "WinSCP client",
            new Dictionary<string, string>() {
                {"de","WinSCP-Client"},
                {"hi","विनएससीपी क्लाइंट"},
                {"fr","Client WinSCP"},
                {"zh-chs","WinSCP客户端 "},
                {"fi","WinSCP -asiakas"},
                {"tr","WinSCP istemcisi"},
                {"cs","Klient WinSCP"},
                {"ja","WinSCPクライアント"},
                {"es","Cliente WinSCP"},
                {"pt","Cliente WinSCP"},
                {"pt-br","Cliente WinSCP"},
                {"sv","WinSCP-klient"},
                {"da","WinSCP-klient"},
                {"ko","WinSCP 클라이언트"},
                {"it","Client WinSCP"},
                {"ru","Клиент WinSCP"}
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
                {"pl","Grupa Urządzeń"},
                {"pt","Grupo de dispositivos"},
                {"nl","Apparaat groep"},
                {"pt-br","Grupo de Dispositivos"},
                {"sv","Enhetsgrupp"},
                {"da","Enhedsgruppe"},
                {"ko","장치 그룹"},
                {"it","Gruppo di dispositivi"},
                {"ru","Группа устройства"}
            }
        },
        {
            "Certificate",
            new Dictionary<string, string>() {
                {"de","Zertifikat"},
                {"hi","प्रमाणपत्र"},
                {"fr","Certificat"},
                {"zh-chs","证书"},
                {"fi","Todistus"},
                {"tr","sertifika"},
                {"cs","Osvědčení"},
                {"ja","証明書"},
                {"es","Certificado"},
                {"pl","Certyfikat"},
                {"pt","Certificado"},
                {"nl","Certificaat"},
                {"pt-br","Certificado"},
                {"sv","Certifikat"},
                {"da","Certifikat"},
                {"ko","증명서"},
                {"it","Certificato"},
                {"ru","Сертификат"}
            }
        },
        {
            "Changing language will close this tool. Are you sure?",
            new Dictionary<string, string>() {
                {"de","Wenn Sie die Sprache ändern, wird dieses Tool geschlossen. Bist du sicher?"},
                {"hi","भाषा बदलने से यह टूल बंद हो जाएगा। क्या आपको यकीन है?"},
                {"fr","Le changement de langue fermera cet outil. Êtes-vous sûr?"},
                {"zh-chs","更改语言将关闭此工具。你确定吗？"},
                {"fi","Kielen vaihtaminen sulkee tämän työkalun. Oletko varma?"},
                {"tr","Dili değiştirmek bu aracı kapatacaktır. Emin misin?"},
                {"cs","Změnou jazyka se tento nástroj zavře. Jsi si jistá?"},
                {"ja","言語を変更すると、このツールは閉じます。本気ですか？"},
                {"es","El cambio de idioma cerrará esta herramienta. ¿Está seguro?"},
                {"pl","Zmiana języka spowoduje zamknięcie tego narzędzia. Czy jesteś pewien?"},
                {"pt","Alterar o idioma fechará esta ferramenta. Tem certeza?"},
                {"nl","Als u de taal wijzigt, wordt deze tool gesloten.Weet je het zeker?"},
                {"pt-br","Alterar o idioma fechará esta ferramenta. Tem certeza?"},
                {"sv","Om du byter språk kommer detta verktyg att stängas. Är du säker?"},
                {"da","Skift af sprog vil lukke dette værktøj. Er du sikker?"},
                {"ko","언어를 변경하면이 도구가 닫힙니다. 확실합니까?"},
                {"it","La modifica della lingua chiuderà questo strumento. Sei sicuro?"},
                {"ru","При изменении языка этот инструмент закроется. Вы уверены?"}
            }
        },
        {
            "Enter the second factor authentication token.",
            new Dictionary<string, string>() {
                {"de","Geben Sie das zweite Faktor-Authentifizierungstoken ein."},
                {"hi","दूसरा कारक प्रमाणीकरण टोकन दर्ज करें।"},
                {"fr","Saisissez le jeton d'authentification du deuxième facteur."},
                {"zh-chs","输入第二个因素身份验证令牌。"},
                {"fi","Syötä toinen tekijän todennustunnus."},
                {"tr","İkinci faktörlü kimlik doğrulama belirtecini girin."},
                {"cs","Zadejte druhý ověřovací token faktoru."},
                {"ja","2要素認証トークンを入力します。"},
                {"es","Ingresa el token del segundo factor de autenticación."},
                {"pl","Wprowadź token drugiego elementu uwierzytelniającego."},
                {"pt","Insira o token de autenticação de segundo fator."},
                {"nl","Voer de tweede factor authenticatie token in."},
                {"pt-br","Insira o token de autenticação de segundo fator."},
                {"sv","Ange den andra faktor-autentiseringstoken."},
                {"da","Indtast den anden faktor godkendelsestoken."},
                {"ko","두 번째 요소 인증 토큰을 입력하십시오."},
                {"it","Immettere il token di autenticazione del secondo fattore."},
                {"ru","Введите токен аутентификации второго фактора."}
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
                {"pl","Szukaj"},
                {"pt","Procurar"},
                {"nl","Zoeken"},
                {"pt-br","Procurar"},
                {"sv","Sök"},
                {"da","Søg"},
                {"ko","검색"},
                {"it","Ricerca"},
                {"ru","Поиск"}
            }
        },
        {
            "Application Link",
            new Dictionary<string, string>() {
                {"de","Bewerbungslink"},
                {"hi","आवेदन लिंक"},
                {"fr","Lien d'application"},
                {"zh-chs","申请链接"},
                {"fi","Sovelluslinkki"},
                {"tr","Başvuru Bağlantısı"},
                {"cs","Odkaz na aplikaci"},
                {"ja","アプリケーションリンク"},
                {"es","Enlace de aplicación"},
                {"pl","Link Aplikacji"},
                {"pt","Link do aplicativo"},
                {"nl","Applicatielink"},
                {"pt-br","Link do aplicativo"},
                {"sv","Applikationslänk"},
                {"da","Applikationslink"},
                {"ko","응용 프로그램 링크"},
                {"it","Collegamento all'applicazione"},
                {"ru","Ссылка на приложение"}
            }
        },
        {
            "Installation",
            new Dictionary<string, string>() {
                {"hi","इंस्टालेशन"},
                {"zh-chs","安装"},
                {"fi","Asennus"},
                {"tr","Kurulum"},
                {"cs","Instalace"},
                {"ja","インストール"},
                {"es","Instalación"},
                {"pl","Instalacja"},
                {"pt","Instalação"},
                {"nl","Installatie"},
                {"pt-br","Instalação"},
                {"ko","설치"},
                {"it","Installazione"},
                {"ru","Установка"}
            }
        },
        {
            "Application",
            new Dictionary<string, string>() {
                {"de","Anwendung"},
                {"hi","आवेदन"},
                {"zh-chs","应用"},
                {"fi","Sovellus"},
                {"tr","Başvuru"},
                {"cs","aplikace"},
                {"ja","応用"},
                {"es","Solicitud"},
                {"pl","Aplikacja"},
                {"pt","Aplicativo"},
                {"nl","Toepassing"},
                {"pt-br","Aplicativo"},
                {"sv","Ansökan"},
                {"da","Applikation"},
                {"ko","신청"},
                {"it","Applicazione"},
                {"ru","заявка"}
            }
        },
        {
            "Application Launch",
            new Dictionary<string, string>() {
                {"de","Anwendungsstart"},
                {"hi","एप्लिकेशन लॉन्च"},
                {"fr","Lancement de l'application"},
                {"zh-chs","应用启动"},
                {"fi","Sovelluksen käynnistäminen"},
                {"tr","Uygulama Başlatma"},
                {"cs","Spuštění aplikace"},
                {"ja","アプリケーションの起動"},
                {"es","Lanzamiento de la aplicación"},
                {"pl","Uruchomienie Aplikacji"},
                {"pt","Lançamento do aplicativo"},
                {"nl","Toepassing starten"},
                {"pt-br","Abrir aplicativo"},
                {"sv","Applikationsstart"},
                {"da","Applikationsstart"},
                {"ko","응용 프로그램 시작"},
                {"it","Lancio dell'applicazione"},
                {"ru","Запуск приложения"}
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
                {"pl","Czas minął"},
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
            "0%",
            new Dictionary<string, string>() {
                {"ja","0％"},
                {"ko","0 %"}
            }
        },
        {
            "Failed to start remote desktop session",
            new Dictionary<string, string>() {
                {"de","Fehler beim Starten der Remote-Desktop-Sitzung"},
                {"hi","दूरस्थ डेस्कटॉप सत्र प्रारंभ करने में विफल"},
                {"fr","Échec du démarrage de la session de bureau à distance"},
                {"zh-chs","无法启动远程桌面会话"},
                {"fi","Etätyöpöytäistunnon käynnistäminen epäonnistui"},
                {"tr","Uzak masaüstü oturumu başlatılamadı"},
                {"cs","Spuštění relace vzdálené plochy se nezdařilo"},
                {"ja","リモートデスクトップセッションの開始に失敗しました"},
                {"es","No se pudo iniciar la sesión de escritorio remoto"},
                {"pl","Nie udało się uruchomić sesji pulpitu zdalnego"},
                {"pt","Falha ao iniciar sessão de área de trabalho remota"},
                {"nl","Kan extern bureaubladsessie niet starten"},
                {"pt-br","Falha ao iniciar sessão de área de trabalho remota"},
                {"sv","Det gick inte att starta fjärrskrivbordssessionen"},
                {"da","Kunne ikke starte fjernskrivebordssession"},
                {"ko","원격 데스크톱 세션을 시작하지 못했습니다."},
                {"it","Impossibile avviare la sessione desktop remoto"},
                {"ru","Не удалось запустить сеанс удаленного рабочего стола"}
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
                {"pt-br","Desconectado"},
                {"sv","Off-line"},
                {"ko","오프라인"},
                {"it","Disconnesso"},
                {"ru","Не в сети"}
            }
        },
        {
            "&Save Mappings...",
            new Dictionary<string, string>() {
                {"de","&Zuordnungen speichern..."},
                {"hi","&मैपिंग सहेजें..."},
                {"fr","&Enregistrer les mappages..."},
                {"zh-chs","保存映射(&S)..."},
                {"fi","& Tallenna kartoitukset ..."},
                {"tr","&Eşlemeleri Kaydet..."},
                {"cs","& Uložit mapování ..."},
                {"ja","＆マッピングの保存..."},
                {"es","&Guardar asignaciones ..."},
                {"pl","&Zapisz Mapowania..."},
                {"pt","& Salvar mapeamentos ..."},
                {"nl","&Toewijzingen opslaan..."},
                {"pt-br","&Salvar mapeamentos ..."},
                {"sv","& Spara kartor ..."},
                {"ko","매핑 저장 ..."},
                {"it","&Salva mappature..."},
                {"ru","&Сохранить сопоставления..."}
            }
        },
        {
            "Ctrl-Alt-Del",
            new Dictionary<string, string>() {
                {"de","Strg-Alt-Entf"}
            }
        },
        {
            "No Devices",
            new Dictionary<string, string>() {
                {"de","Keine Geräte"},
                {"hi","कोई उपकरण नहीं"},
                {"fr","Aucun appareil"},
                {"zh-chs","没有设备"},
                {"fi","Ei laitteita"},
                {"tr","Cihaz Yok"},
                {"cs","Žádná zařízení"},
                {"ja","デバイスなし"},
                {"es","Sin Dispositivos"},
                {"pl","Brak Urządzeń"},
                {"pt","Sem dispositivos"},
                {"nl","Geen apparaten"},
                {"pt-br","Sem dispositivos"},
                {"sv","Inga enheter"},
                {"da","Ingen enheder"},
                {"ko","장치 없음"},
                {"it","Nessun Dispositivo"},
                {"ru","Нет устройств"}
            }
        },
        {
            "Click ok to register MeshCentral Router on your system as the handler for the \"mcrouter://\" protocol. This will allow the MeshCentral web site to launch this application when needed.",
            new Dictionary<string, string>() {
                {"de","Klicken Sie auf OK, um MeshCentral Router auf Ihrem System als Handler für das Protokoll \"mcrouter://\" zu registrieren. Dadurch kann die MeshCentral-Website diese Anwendung bei Bedarf starten."},
                {"hi","MeshCentral राउटर को अपने सिस्टम पर \"mcrouter: //\" प्रोटोकॉल के लिए हैंडलर के रूप में पंजीकृत करने के लिए ओके पर क्लिक करें। यह मेशसेंट्रल वेब साइट को जरूरत पड़ने पर इस एप्लिकेशन को लॉन्च करने की अनुमति देगा।"},
                {"fr","Cliquez sur ok pour enregistrer MeshCentral Router sur votre système en tant que gestionnaire du protocole « mcrouter:// ». Cela permettra au site Web MeshCentral de lancer cette application en cas de besoin."},
                {"zh-chs","单击确定在您的系统上注册 MeshCentral Router 作为“mcrouter://”协议的处理程序。这将允许 MeshCentral 网站在需要时启动此应用程序。"},
                {"fi","Napsauta ok rekisteröidäksesi MeshCentral Router järjestelmääsi \"mcrouter: //\" -protokollan käsittelijäksi. Tämä antaa MeshCentral -verkkosivuston käynnistää tämän sovelluksen tarvittaessa."},
                {"tr","MeshCentral Router'ı sisteminizde \"mcrouter://\" protokolü için işleyici olarak kaydetmek için Tamam'a tıklayın. Bu, MeshCentral web sitesinin gerektiğinde bu uygulamayı başlatmasını sağlayacaktır."},
                {"cs","Kliknutím na ok zaregistrujete MeshCentral Router ve vašem systému jako obslužný nástroj pro protokol „mcrouter: //“. To umožní webu MeshCentral spustit tuto aplikaci v případě potřeby."},
                {"ja","[OK]をクリックして、システムにMeshCentralルーターを「mcrouter：//」プロトコルのハンドラーとして登録します。これにより、MeshCentralWebサイトは必要に応じてこのアプリケーションを起動できるようになります。"},
                {"es","Haz clic en Aceptar para registrar MeshCentral Router en tu sistema como el controlador del protocolo \"mcrouter: //\". Esto permitirá que el sitio web de MeshCentral inicie esta aplicación cuando sea necesario."},
                {"pl","Kliknij ok, aby zarejestrować MeshCentral Router w twoim systemie jako obsługujący protokół \"mcrouter://\". Pozwoli to stronie MeshCentral na uruchomienie tej aplikacji w razie potrzeby."},
                {"pt","Clique em ok para registrar o MeshCentral Router em seu sistema como o manipulador do protocolo \"mcrouter: //\". Isso permitirá que o site MeshCentral inicie esse aplicativo quando necessário."},
                {"nl","Klik op ok om MeshCentral Router op uw systeem te registreren als de handler voor het \"mcrouter://\" protocol. Hierdoor kan de MeshCentral-website deze applicatie starten wanneer dat nodig is."},
                {"pt-br","Clique em ok para registrar o MeshCentral Router em seu sistema como o gerenciador do protocolo \"mcrouter: //\". Isso permitirá que o site MeshCentral inicie esse aplicativo quando necessário."},
                {"sv","Klicka på ok för att registrera MeshCentral Router på ditt system som hanterare för \"mcrouter: //\" -protokollet. Detta gör att MeshCentral-webbplatsen kan starta den här applikationen vid behov."},
                {"da","Klik på ok for at registrere MeshCentral Router på dit system for håndtering af \"mcrouter://\"-protokollen. Dette vil gøre det muligt for MeshCentral-webstedet at starte denne applikation, når det er nødvendigt."},
                {"ko","확인을 클릭하여 \"mcrouter : //\"프로토콜의 핸들러로 시스템에 MeshCentral 라우터를 등록하십시오. 이렇게하면 필요할 때 MeshCentral 웹 사이트에서이 응용 프로그램을 시작할 수 있습니다."},
                {"it","Fare clic su OK per registrare MeshCentral Router sul sistema come gestore per il protocollo \"mcrouter://\". Ciò consentirà al sito Web MeshCentral di avviare questa applicazione quando necessario."},
                {"ru","Нажмите ОК, чтобы зарегистрировать MeshCentral Router в вашей системе в качестве обработчика протокола «mcrouter: //». Это позволит веб-сайту MeshCentral запускать это приложение при необходимости."}
            }
        },
        {
            "Show &Group Names",
            new Dictionary<string, string>() {
                {"de","&Gruppennamen anzeigen"},
                {"hi","समूह के नाम दिखाएं"},
                {"fr","Afficher les noms de &groupes"},
                {"zh-chs","显示组名称(&G)"},
                {"fi","Näytä & ryhmien nimet"},
                {"tr","&Grup Adlarını Göster"},
                {"cs","Zobrazit a seskupit jména"},
                {"ja","＆グループ名を表示"},
                {"es","Mostrar y nombres de grupos"},
                {"pl","Pokaż Nazwy &Grup"},
                {"pt","Mostrar nomes de grupos"},
                {"nl","Toon &groepsnamen"},
                {"pt-br","Mostrar nomes de grupos"},
                {"sv","Visa & gruppnamn"},
                {"da","Vis &Group navne"},
                {"ko","그룹 이름 표시"},
                {"it","Mostra i nomi dei gruppi"},
                {"ru","Показать и названия групп"}
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
                {"pl","Szybko"},
                {"pt","Rápido"},
                {"nl","Snel"},
                {"pt-br","Rápido"},
                {"sv","Snabb"},
                {"da","Hurtig"},
                {"ko","빠른"},
                {"it","Veloce"},
                {"ru","Быстро"}
            }
        },
        {
            "This server presented a un-trusted certificate.  This may indicate that this is not the correct server or that the server does not have a valid certificate. It is not recommanded, but you can press the ignore button to continue connection to this server.",
            new Dictionary<string, string>() {
                {"de","Dieser Server hat ein nicht vertrauenswürdiges Zertifikat vorgelegt. Dies kann darauf hinweisen, dass dies nicht der richtige Server ist oder dass der Server nicht über ein gültiges Zertifikat verfügt. Es wird nicht empfohlen, aber Sie können die Ignorieren-Taste drücken, um die Verbindung zu diesem Server fortzusetzen."},
                {"hi","इस सर्वर ने एक अविश्वसनीय प्रमाणपत्र प्रस्तुत किया। यह संकेत दे सकता है कि यह सही सर्वर नहीं है या सर्वर के पास वैध प्रमाणपत्र नहीं है। यह अनुशंसित नहीं है, लेकिन आप इस सर्वर से कनेक्शन जारी रखने के लिए अनदेखा करें बटन दबा सकते हैं।"},
                {"fr","Ce serveur a présenté un certificat non approuvé. Cela peut indiquer qu'il ne s'agit pas du bon serveur ou que le serveur n'a pas de certificat valide. Ce n'est pas recommandé, mais vous pouvez appuyer sur le bouton ignorer pour continuer la connexion à ce serveur."},
                {"zh-chs","此服务器提供了不受信任的证书。这可能表明这不是正确的服务器或服务器没有有效的证书。不推荐，但您可以按忽略按钮继续连接到此服务器。"},
                {"fi","Tämä palvelin esitti epäluotettavan varmenteen. Tämä voi tarkoittaa, että tämä ei ole oikea palvelin tai että palvelimella ei ole kelvollista varmennetta. Sitä ei suositella, mutta voit jatkaa yhteyttä tähän palvelimeen painamalla ohituspainiketta."},
                {"tr","Bu sunucu güvenilmeyen bir sertifika sundu. Bu, bunun doğru sunucu olmadığını veya sunucunun geçerli bir sertifikası olmadığını gösterebilir. Tavsiye edilmez, ancak bu sunucuyla bağlantıya devam etmek için yoksay düğmesine basabilirsiniz."},
                {"cs","Tento server předložil nedůvěryhodný certifikát. Může to znamenat, že to není správný server nebo že server nemá platný certifikát. Nedoporučuje se to, ale můžete pokračovat v připojení k tomuto serveru stisknutím tlačítka ignorovat."},
                {"ja","このサーバーは、信頼できない証明書を提示しました。これは、これが正しいサーバーではないか、サーバーに有効な証明書がないことを示している可能性があります。推奨されていませんが、無視ボタンを押してこのサーバーへの接続を続行できます。"},
                {"es","Este servidor presentó un certificado no confiable. Esto puede indicar que este no es el servidor correcto o que el servidor no tiene un certificado válido. No se recomienda, pero puedes presionar el botón ignorar para continuar la conexión a este servidor."},
                {"pl","Ten serwer przedstawił niezaufany certyfikat.  Może to oznaczać, że nie jest to właściwy serwer lub że serwer nie posiada ważnego certyfikatu. Nie jest to zalecane, ale możesz nacisnąć przycisk ignoruj, aby kontynuować połączenie z tym serwerem."},
                {"pt","Este servidor apresentou um certificado não confiável. Isso pode indicar que este não é o servidor correto ou que o servidor não possui um certificado válido. Não é recomendado, mas você pode pressionar o botão de ignorar para continuar a conexão com este servidor."},
                {"nl","Deze server heeft een niet-vertrouwd certificaat gepresenteerd. Dit kan erop wijzen dat dit niet de juiste server is of dat de server geen geldig certificaat heeft. Het wordt niet aanbevolen, maar u kunt op de negeren drukken om de verbinding met deze server voort te zetten."},
                {"pt-br","Este servidor apresentou um certificado não confiável. Isso pode indicar que este não é o servidor correto ou que o servidor não possui um certificado válido. Não é recomendado, mas você pode pressionar o botão de ignorar para continuar a conexão com este servidor."},
                {"sv","Denna server presenterade ett otillförlitligt certifikat. Detta kan indikera att detta inte är rätt server eller att servern inte har ett giltigt certifikat. Det rekommenderas inte, men du kan trycka på ignorera-knappen för att fortsätta anslutningen till den här servern."},
                {"da","Denne server præsenterede et certifikat, der ikke er tillid til. Dette kan indikere, at dette ikke er den rigtige server, eller at serveren ikke har et gyldigt certifikat. Det anbefales ikke, men du kan trykke på ignorer-knappen for at fortsætte forbindelsen til denne server."},
                {"ko","이 서버는 신뢰할 수없는 인증서를 제공했습니다. 이는 올바른 서버가 아니거나 서버에 유효한 인증서가 없음을 나타낼 수 있습니다. 권장되지는 않지만 무시 버튼을 눌러이 서버에 계속 연결할 수 있습니다."},
                {"it","Questo server ha presentato un certificato non attendibile.Questo potrebbe indicare che questo non è il server corretto o che il server non ha un certificato valido.Non è raccomandato, ma puoi premere il pulsante Ignora per continuare la connessione a questo server."},
                {"ru","Этот сервер представил ненадежный сертификат. Это может означать, что это неправильный сервер или что у сервера нет действующего сертификата. Это не рекомендуется, но вы можете нажать кнопку игнорирования, чтобы продолжить подключение к этому серверу."}
            }
        },
        {
            "R&efresh",
            new Dictionary<string, string>() {
                {"de","Aktualisierung"},
                {"hi","ताज़ा करें"},
                {"fr","Rafraîchir"},
                {"zh-chs","刷新"},
                {"fi","Virkistää"},
                {"tr","&yenile"},
                {"cs","Obnovit"},
                {"ja","R＆efresh"},
                {"es","Actualizar"},
                {"pl","Odśwież (R&)"},
                {"pt","R & efresh"},
                {"nl","Vernieuwen"},
                {"pt-br","Atualizar"},
                {"sv","R & efresh"},
                {"ko","새롭게 하다"},
                {"ru","R & efresh"}
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
                {"pl","Skalowanie"},
                {"pt","Dimensionamento"},
                {"nl","Schalen"},
                {"pt-br","Dimensionamento"},
                {"sv","Skalning"},
                {"da","Skalering"},
                {"ko","비율"},
                {"it","Ridimensionamento"},
                {"ru","Маштабирование"}
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
                {"pl","Wysłać token na zarejestrowany numer telefonu?"},
                {"pt","Enviar token para o número de telefone registrado?"},
                {"nl","Token naar geregistreerd telefoonnummer verzenden?"},
                {"pt-br","Enviar token para o número de telefone registrado?"},
                {"sv","Skicka token till registrerat telefonnummer?"},
                {"da","Send token til registreret telefonnummer?"},
                {"ko","등록된 휴대폰 번호로 토큰을 보내시겠습니까?"},
                {"it","Invia token al numero di telefono registrato?"},
                {"ru","Отправить токен на зарегистрированный номер телефона?"}
            }
        },
        {
            "{0} Kbytes/sec",
            new Dictionary<string, string>() {
                {"de","{0} Kbyte/s"},
                {"hi","{0} किलोबाइट/सेकंड"},
                {"zh-chs","{0} KB/秒"},
                {"fi","{0} Kt/sek"},
                {"tr","{0} Kbayt/sn"},
                {"cs","{0} kB/s"},
                {"ja","{0}キロバイト/秒"},
                {"es","{0} Kbytes / seg"},
                {"pl","{0} KB/s"},
                {"pt","{0} Kbytes / s"},
                {"pt-br","{0} Kbytes / seg"},
                {"sv","{0} Kbyte/sek"},
                {"da","{0} Kbytes/sek"},
                {"ko","{0}KB/초"},
                {"it","{0} Kbyte/sec"},
                {"ru","{0} Кбайт / с"}
            }
        },
        {
            "Ignore",
            new Dictionary<string, string>() {
                {"de","Ignorieren"},
                {"hi","नज़रअंदाज़ करना"},
                {"fr","Ignorer"},
                {"zh-chs","忽略"},
                {"fi","Jättää huomiotta"},
                {"tr","Görmezden gelmek"},
                {"cs","Ignorovat"},
                {"ja","無視"},
                {"es","Ignorar"},
                {"pl","Ignoruj"},
                {"pt","Ignorar"},
                {"nl","Negeren"},
                {"pt-br","Ignorar"},
                {"sv","Strunta i"},
                {"da","Ignorér"},
                {"ko","무시"},
                {"it","Ignorare"},
                {"ru","Игнорировать"}
            }
        },
        {
            "Port Mapping",
            new Dictionary<string, string>() {
                {"de","Port-Mapping"},
                {"hi","पोर्ट मानचित्रण"},
                {"fr","Mappage des ports"},
                {"zh-chs","端口映射"},
                {"fi","Portin kartoitus"},
                {"tr","Liman Haritalama"},
                {"cs","Mapování portů"},
                {"ja","ポートマッピング"},
                {"es","Mapeo de Puertos"},
                {"pl","Mapowanie Portu"},
                {"pt","Mapeamento de portas"},
                {"nl","Poorttoewijzing"},
                {"pt-br","Mapeamento de portas"},
                {"sv","Portmappning"},
                {"ko","포트 매핑"},
                {"it","Mappatura delle porte"},
                {"ru","Отображение портов"}
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
                {"pl","Aktualizacja"},
                {"pt","Atualizar"},
                {"nl","Bijwerken"},
                {"pt-br","Atualizar"},
                {"sv","Uppdatering"},
                {"da","Opdatering"},
                {"ko","개조하다"},
                {"it","Aggiornamenti"},
                {"ru","Обновить"}
            }
        },
        {
            "RDP",
            new Dictionary<string, string>() {
                {"hi","आरडीपी"}
            }
        },
        {
            "Almost done",
            new Dictionary<string, string>() {
                {"de","Fast fertig"},
                {"hi","लगभग हो गया"},
                {"fr","Bientôt terminé"},
                {"zh-chs","快完成了"},
                {"fi","Melkein valmis"},
                {"tr","Neredeyse bitti"},
                {"cs","Skoro hotovo"},
                {"ja","ほぼ完了しました"},
                {"es","Casi termino"},
                {"pl","Prawie skończone"},
                {"pt","Quase pronto"},
                {"nl","Bijna klaar"},
                {"pt-br","Quase Pronto"},
                {"sv","Nästan klar"},
                {"da","Næsten klar"},
                {"ko","거의 완료"},
                {"it","Quasi fatto"},
                {"ru","Почти сделано"}
            }
        },
        {
            "Remove {0} items?",
            new Dictionary<string, string>() {
                {"de","{0} Elemente entfernen?"},
                {"hi","{0} आइटम निकालें?"},
                {"fr","Supprimer {0} éléments ?"},
                {"zh-chs","删除 {0} 项？"},
                {"fi","Poistetaanko {0} kohdetta?"},
                {"tr","{0} öğe kaldırılsın mı?"},
                {"cs","Odstranit {0} položek?"},
                {"ja","{0}アイテムを削除しますか？"},
                {"es","¿Eliminar {0} elementos?"},
                {"pl","Usunać {0} elementów?"},
                {"pt","Remover {0} itens?"},
                {"nl","{0} items verwijderen?"},
                {"pt-br","Remover {0} itens?"},
                {"sv","Ta bort {0} objekt?"},
                {"da","Fjern {0} elementer?"},
                {"ko","{0} 개 항목을 삭제 하시겠습니까?"},
                {"it","Rimuovere {0} elementi?"},
                {"ru","Удалить элементы ({0})?"}
            }
        },
        {
            "Remote Port",
            new Dictionary<string, string>() {
                {"de","Remote-Port"},
                {"hi","रिमोट पोर्ट"},
                {"fr","Port distant"},
                {"zh-chs","远程端口"},
                {"fi","Etäportti"},
                {"tr","Uzak Bağlantı Noktası"},
                {"cs","Vzdálený port"},
                {"ja","リモートポート"},
                {"es","Puerto Remoto"},
                {"pl","Zdalny Port"},
                {"pt","Porta Remota"},
                {"nl","Externe poort"},
                {"pt-br","Porta Remota"},
                {"sv","Fjärrport"},
                {"da","Fjernport"},
                {"ko","원격 포트"},
                {"it","Porta remota"},
                {"ru","Удаленный порт"}
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
                {"tr","İptal"},
                {"cs","Storno"},
                {"ja","キャンセル"},
                {"es","Cancelar"},
                {"pl","Anuluj"},
                {"pt","Cancelar"},
                {"nl","Annuleren"},
                {"pt-br","Cancelar"},
                {"sv","Avbryt"},
                {"da","Annuller"},
                {"ko","취소"},
                {"it","Annulla"},
                {"ru","Отмена"}
            }
        },
        {
            "Alternative Port",
            new Dictionary<string, string>() {
                {"de","Alternativer Port"},
                {"hi","वैकल्पिक बंदरगाह"},
                {"fr","Port alternatif"},
                {"zh-chs","替代端口"},
                {"fi","Vaihtoehtoinen portti"},
                {"tr","Alternatif Bağlantı Noktası"},
                {"cs","Alternativní port"},
                {"ja","代替ポート"},
                {"es","Puerto Alternativo"},
                {"pl","Alternatywny Port"},
                {"pt","Porto Alternativo"},
                {"nl","Alternatieve poort"},
                {"pt-br","Porta Alternativa"},
                {"sv","Alternativ hamn"},
                {"da","Alternativ port"},
                {"ko","대체 포트"},
                {"it","Porta alternativa"},
                {"ru","Альтернативный порт"}
            }
        },
        {
            "Toggle zoom-to-fit mode",
            new Dictionary<string, string>() {
                {"de","Zoom-to-Fit-Modus umschalten"},
                {"hi","ज़ूम-टू-फ़िट मोड टॉगल करें"},
                {"fr","Basculer en mode zoom pour ajuster"},
                {"zh-chs","切换缩放至适合模式"},
                {"fi","Vaihda sovitustila"},
                {"tr","Sığdırmak için yakınlaştırma modunu değiştir"},
                {"cs","Přepnout režim přiblížení a přizpůsobení"},
                {"ja","ズームしてフィットモードを切り替えます"},
                {"es","Alternar el modo de zoom para ajustar"},
                {"pl","Przełączanie trybu powiększania na dopasowanie"},
                {"pt","Alternar modo de zoom para ajustar"},
                {"nl","Schakel inzoemen naar passend modus in"},
                {"pt-br","Alternar modo de zoom para ajustar"},
                {"sv","Växla zoom-to-fit-läge"},
                {"da","Skift zoom-for-tilpasning tilstand"},
                {"ko","확대 / 축소 모드 전환"},
                {"it","Attiva/disattiva la modalità zoom per adattare"},
                {"ru","Переключить режим масштабирования по размеру"}
            }
        },
        {
            "Remote Files...",
            new Dictionary<string, string>() {
                {"de","Remote-Dateien..."},
                {"hi","दूरस्थ फ़ाइलें..."},
                {"fr","Fichiers distants..."},
                {"zh-chs","远程文件..."},
                {"fi","Etätiedostot ..."},
                {"tr","Uzak Dosyalar..."},
                {"cs","Vzdálené soubory ..."},
                {"ja","リモートファイル..."},
                {"es","Archivos Remotos ..."},
                {"pl","Zdalne Pliki..."},
                {"pt","Arquivos remotos ..."},
                {"nl","Externe bestanden..."},
                {"pt-br","Arquivos remotos ..."},
                {"sv","Fjärrfiler ..."},
                {"da","Fjernfiler..."},
                {"ko","원격 파일 ..."},
                {"it","File remoti..."},
                {"ru","Удаленные файлы ..."}
            }
        },
        {
            "HTTPS",
            new Dictionary<string, string>() {
                {"hi","HTTPS के"}
            }
        },
        {
            "Compressed Network Traffic",
            new Dictionary<string, string>() {
                {"de","Komprimierter Netzwerkverkehr"},
                {"hi","संपीडित नेटवर्क यातायात"},
                {"fr","Trafic réseau compressé"},
                {"zh-chs","压缩网络流量"},
                {"fi","Pakattu verkkoliikenne"},
                {"tr","Sıkıştırılmış Ağ Trafiği"},
                {"cs","Komprimovaný síťový provoz"},
                {"ja","圧縮されたネットワークトラフィック"},
                {"es","Tráfico de red comprimido"},
                {"pl","Skompresowany Ruch Sieciowy"},
                {"pt","Tráfego de rede comprimido"},
                {"nl","Gecomprimeerd netwerkverkeer"},
                {"pt-br","Tráfego de rede comprimido"},
                {"sv","Komprimerad nätverkstrafik"},
                {"da","Komprimeret netværkstrafik"},
                {"ko","압축 된 네트워크 트래픽"},
                {"it","Traffico di rete compresso"},
                {"ru","Сжатый сетевой трафик"}
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
                {"pl","Odebrano nieprawidłowe dane sieciowe"},
                {"pt","Dados de rede inválidos recebidos"},
                {"nl","Ongeldige netwerkgegevens ontvangen"},
                {"pt-br","Dados de rede inválidos recebidos"},
                {"sv","Mottog ogiltig nätverksdata"},
                {"da","Modtog ugyldige netværksdata"},
                {"ko","잘못된 네트워크 데이터를 받았습니다."},
                {"it","Dati di rete non validi ricevuti"},
                {"ru","Получены неверные сетевые данные"}
            }
        },
        {
            "UDP",
            new Dictionary<string, string>() {
                {"hi","यूडीपी"}
            }
        },
        {
            "Invalid username or password",
            new Dictionary<string, string>() {
                {"de","ungültiger Benutzername oder Passwort"},
                {"hi","अमान्य उपयोगकर्ता नाम या पासवर्ड"},
                {"fr","Nom d'utilisateur ou mot de passe invalide"},
                {"zh-chs","无效的用户名或密码"},
                {"fi","väärä käyttäjänimi tai salasana"},
                {"tr","Geçersiz kullanıcı adı veya şifre"},
                {"cs","neplatné uživatelské jméno nebo heslo"},
                {"ja","無効なユーザー名またはパスワード"},
                {"es","Usuario o contraseña invalido"},
                {"pl","Nieprawidłowa nazwa użytkownika lub hasło"},
                {"pt","nome de usuário ou senha inválidos"},
                {"nl","Ongeldige gebruikersnaam of wachtwoord"},
                {"pt-br","nome de usuário ou senha inválidos"},
                {"sv","Ogiltigt användarnamn eller lösenord"},
                {"da","Ugyldigt brugernavn eller kodeord"},
                {"ko","잘못된 사용자 이름 또는 비밀번호"},
                {"it","Username o password non validi"},
                {"ru","неправильное имя пользователя или пароль"}
            }
        },
        {
            "Bind local port to all network interfaces",
            new Dictionary<string, string>() {
                {"de","Binden Sie den lokalen Port an alle Netzwerkschnittstellen"},
                {"hi","स्थानीय पोर्ट को सभी नेटवर्क इंटरफेस से बाइंड करें"},
                {"fr","Lier le port local à toutes les interfaces réseau"},
                {"zh-chs","将本地端口绑定到所有网络接口"},
                {"fi","Sido paikallinen portti kaikkiin verkkoliitäntöihin"},
                {"tr","Yerel bağlantı noktasını tüm ağ arabirimlerine bağlayın"},
                {"cs","Vázat místní port na všechna síťová rozhraní"},
                {"ja","ローカルポートをすべてのネットワークインターフェイスにバインドする"},
                {"es","Vincular el puerto local a todas las interfaces de red"},
                {"pl","Przypisz port lokalny do wszystkich interfejsów sieciowych"},
                {"pt","Vincule a porta local a todas as interfaces de rede"},
                {"nl","Bind lokale poort aan alle netwerkinterfaces"},
                {"pt-br","Vincule a porta local a todas as interfaces de rede"},
                {"sv","Binda lokal port till alla nätverksgränssnitt"},
                {"da","Bind lokal port til alle netværksinterfaces"},
                {"ko","모든 네트워크 인터페이스에 로컬 포트 ​​바인딩"},
                {"it","Associa la porta locale a tutte le interfacce di rete"},
                {"ru","Привязать локальный порт ко всем сетевым интерфейсам"}
            }
        },
        {
            "Outgoing Compression",
            new Dictionary<string, string>() {
                {"de","Ausgehende Komprimierung"},
                {"hi","आउटगोइंग संपीड़न"},
                {"fr","Compression sortante"},
                {"zh-chs","输出压缩"},
                {"fi","Lähtevä pakkaus"},
                {"tr","Giden Sıkıştırma"},
                {"cs","Odchozí komprese"},
                {"ja","発信圧縮"},
                {"es","Compresión Saliente"},
                {"pl","Kompresja Wychodząca"},
                {"pt","Compressão de saída"},
                {"nl","Uitgaande compressie"},
                {"pt-br","Compressão de saída"},
                {"sv","Utgående kompression"},
                {"da","Udgående kompression"},
                {"ko","나가는 압축"},
                {"it","Compressione in uscita"},
                {"ru","Исходящее сжатие"}
            }
        },
        {
            "Mapping Settings",
            new Dictionary<string, string>() {
                {"de","Zuordnungseinstellungen"},
                {"hi","मैपिंग सेटिंग्स"},
                {"fr","Paramètres de mappage"},
                {"zh-chs","映射设置"},
                {"fi","Kartoitusasetukset"},
                {"tr","Eşleme Ayarları"},
                {"cs","Nastavení mapování"},
                {"ja","マッピング設定"},
                {"es","Configuración de Mapeo"},
                {"pl","Ustawienia Mapowania"},
                {"pt","Configurações de mapeamento"},
                {"nl","Kaartinstellingen"},
                {"pt-br","Configurações de mapeamento"},
                {"sv","Kartläggningsinställningar"},
                {"da","Indstillinger for kortlægning"},
                {"ko","매핑 설정"},
                {"it","Impostazioni di mappatura"},
                {"ru","Настройки отображения"}
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
                {"pl","Powrót"},
                {"pt","Voltar"},
                {"nl","Terug"},
                {"pt-br","Voltar"},
                {"sv","Tillbaka"},
                {"da","Tilbage"},
                {"ko","뒤로"},
                {"it","Indietro"},
                {"ru","Назад"}
            }
        },
        {
            "{0} bytes/sec",
            new Dictionary<string, string>() {
                {"de","{0} Byte/Sek."},
                {"hi","{0} बाइट्स/सेकंड"},
                {"zh-chs","{0} 字节/秒"},
                {"fi","{0} tavua/sek"},
                {"tr","{0} bayt/sn"},
                {"cs","{0} bajtů/s"},
                {"ja","{0}バイト/秒"},
                {"es","{0} bytes / seg"},
                {"pl","{0} bajtów/s"},
                {"pt","{0} bytes / s"},
                {"pt-br","{0} bytes / seg"},
                {"sv","{0} byte/sek"},
                {"da","{0} bytes/sek"},
                {"ko","{0}바이트/초"},
                {"it","{0} byte/sec"},
                {"ru","{0} байт / сек"}
            }
        },
        {
            "Enhanced keyboard capture",
            new Dictionary<string, string>() {
                {"de","Verbesserte Tastaturerfassung"},
                {"hi","उन्नत कीबोर्ड कैप्चर"},
                {"fr","Capture de clavier améliorée"},
                {"zh-chs","增强的键盘捕获"},
                {"fi","Parannettu näppäimistön sieppaus"},
                {"tr","Gelişmiş klavye yakalama"},
                {"cs","Vylepšené snímání klávesnice"},
                {"ja","強化されたキーボードキャプチャ"},
                {"es","Captura de teclado mejorada"},
                {"pl","Ulepszone przechwytywanie klawiatury"},
                {"pt","Captura de teclado aprimorada"},
                {"nl","Verbeterde toetsenbordopname"},
                {"pt-br","Captura de teclado aprimorada"},
                {"sv","Förbättrad tangentbordsfångst"},
                {"da","Forbedret tastatur optagelse"},
                {"ko","향상된 키보드 캡처"},
                {"it","Acquisizione avanzata della tastiera"},
                {"ru","Улучшенный захват клавиатуры"}
            }
        },
        {
            "Display connection statistics",
            new Dictionary<string, string>() {
                {"de","Verbindungsstatistik anzeigen"},
                {"hi","कनेक्शन आंकड़े प्रदर्शित करें"},
                {"fr","Afficher les statistiques de connexion"},
                {"zh-chs","显示连接统计"},
                {"fi","Näytä yhteystilastot"},
                {"tr","Bağlantı istatistiklerini görüntüle"},
                {"cs","Zobrazit statistiky připojení"},
                {"ja","接続統計を表示する"},
                {"es","Mostrar estadísticas de conexión"},
                {"pl","Wyświetl statystyki połączeń"},
                {"pt","Exibir estatísticas de conexão"},
                {"nl","Verbindingsstatistieken weergeven"},
                {"pt-br","Exibir estatísticas de conexão"},
                {"sv","Visa anslutningsstatistik"},
                {"da","Vis forbindelsesstatistikker"},
                {"ko","연결 통계 표시"},
                {"it","Visualizza le statistiche di connessione"},
                {"ru","Показать статистику подключений"}
            }
        },
        {
            "Install...",
            new Dictionary<string, string>() {
                {"de","Installieren..."},
                {"hi","इंस्टॉल..."},
                {"fr","Installer..."},
                {"zh-chs","安装..."},
                {"fi","Asentaa..."},
                {"tr","Düzenlemek..."},
                {"cs","Nainstalujte..."},
                {"ja","インストール..."},
                {"es","Instalar en pc..."},
                {"pl","Instaluj..."},
                {"pt","Instalar..."},
                {"nl","Installeren..."},
                {"pt-br","Instalar..."},
                {"sv","Installera..."},
                {"da","Installere..."},
                {"ko","설치..."},
                {"it","Installare..."},
                {"ru","Установить..."}
            }
        },
        {
            "MQTT",
            new Dictionary<string, string>() {
                {"it","MQTT "}
            }
        },
        {
            "Server information",
            new Dictionary<string, string>() {
                {"de","Serverinformation"},
                {"hi","सर्वर जानकारी"},
                {"fr","Informations sur le serveur"},
                {"zh-chs","服务器信息"},
                {"fi","Palvelimen tiedot"},
                {"tr","Sunucu bilgileri"},
                {"cs","Informace o serveru"},
                {"ja","サーバー情報"},
                {"es","Información del Servidor"},
                {"pl","Informacje o Serwerze"},
                {"pt","Informação do servidor"},
                {"nl","Server informatie"},
                {"pt-br","Informação do servidor"},
                {"sv","Serverinformation"},
                {"ko","서버 정보"},
                {"it","Informazioni sul server"},
                {"ru","Информация о сервере"}
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
                {"pl","Użyj Mapy Klawiatury Zdalnego Urządzenia"},
                {"pt","Use o mapa do teclado remoto"},
                {"nl","Gebruik de externe toetsenbord instelling"},
                {"pt-br","Use o mapa do teclado remoto"},
                {"sv","Använd Remote Keyboard Map"},
                {"da","Brug fjerntastatur mapning"},
                {"ko","원격 키보드 맵 사용"},
                {"it","Usa mappa tastiera remota "},
                {"ru","Использовать карту удаленной клавиатуры"}
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
                {"pl","Rozłącz"},
                {"pt","Desconectar"},
                {"nl","Verbreken"},
                {"pt-br","Desconectar"},
                {"sv","Koppla ifrån"},
                {"da","Afbryd"},
                {"ko","연결 해제"},
                {"it","Disconnetti"},
                {"ru","Разъединить"}
            }
        },
        {
            "Overwrite 1 file?",
            new Dictionary<string, string>() {
                {"de","1 Datei überschreiben?"},
                {"hi","1 फ़ाइल को अधिलेखित करें?"},
                {"fr","Écraser 1 fichier ?"},
                {"zh-chs","覆盖 1 个文件？"},
                {"fi","Korvataanko 1 tiedosto?"},
                {"tr","1 dosyanın üzerine yazılsın mı?"},
                {"cs","Přepsat 1 soubor?"},
                {"ja","1つのファイルを上書きしますか？"},
                {"es","¿Sobrescribir 1 archivo?"},
                {"pl","Zastąpić 1 plik?"},
                {"pt","Substituir 1 arquivo?"},
                {"nl","1 bestand overschrijven?"},
                {"pt-br","Sobrescrever 1 arquivo?"},
                {"sv","Skriv över 1 fil?"},
                {"da","Overskriv 1 fil?"},
                {"ko","파일 1개를 덮어쓰시겠습니까?"},
                {"it","Sovrascrivere 1 file?"},
                {"ru","Перезаписать 1 файл?"}
            }
        },
        {
            "Send Ctrl-Alt-Del to remote device",
            new Dictionary<string, string>() {
                {"de","Strg-Alt-Entf an Remote-Gerät senden"},
                {"hi","रिमोट डिवाइस पर Ctrl-Alt-Del भेजें"},
                {"fr","Envoyer Ctrl-Alt-Suppr à l'appareil distant"},
                {"zh-chs","发送 Ctrl-Alt-Del 到远程设备"},
                {"fi","Lähetä Ctrl-Alt-Del etälaitteeseen"},
                {"tr","Uzak cihaza Ctrl-Alt-Del gönder"},
                {"cs","Odeslat Ctrl-Alt-Del na vzdálené zařízení"},
                {"ja","Ctrl-Alt-Delをリモートデバイスに送信します"},
                {"es","Enviar Ctrl-Alt-Del al dispositivo remoto"},
                {"pl","Wyślij Ctrl-Alt-Del do urządzenia zdalnego"},
                {"pt","Envie Ctrl-Alt-Del para o dispositivo remoto"},
                {"nl","Stuur Ctrl-Alt-Del naar extern apparaat"},
                {"pt-br","Envie Ctrl-Alt-Del para o dispositivo remoto"},
                {"sv","Skicka Ctrl-Alt-Del till fjärrenhet"},
                {"da","Send Ctrl-Alt-Del til fjernenhed"},
                {"ko","Ctrl-Alt-Del을 원격 장치로 보내기"},
                {"it","Invia Ctrl-Alt-Canc al dispositivo remoto"},
                {"ru","Отправить Ctrl-Alt-Del на удаленное устройство"}
            }
        },
        {
            "Show on system tray",
            new Dictionary<string, string>() {
                {"de","In der Taskleiste anzeigen"},
                {"hi","सिस्टम ट्रे पर दिखाएं"},
                {"fr","Afficher sur la barre d'état système"},
                {"zh-chs","在系统托盘上显示"},
                {"fi","Näytä ilmaisinalueella"},
                {"tr","Sistem tepsisinde göster"},
                {"cs","Zobrazit na hlavním panelu"},
                {"ja","システムトレイに表示"},
                {"es","Mostrar en la bandeja del sistema"},
                {"pl","Pokaż w zasobniku systemowym"},
                {"pt","Mostrar na bandeja do sistema"},
                {"nl","Weergeven in systeemvak"},
                {"pt-br","Mostrar na bandeja do sistema"},
                {"sv","Visa i systemfältet"},
                {"da","Vis i systembakken"},
                {"ko","시스템 트레이에 표시"},
                {"it","Mostra sulla barra delle applicazioni"},
                {"ru","Показать на панели задач"}
            }
        },
        {
            "File Transfer",
            new Dictionary<string, string>() {
                {"de","Datei Übertragung"},
                {"hi","फ़ाइल स्थानांतरण"},
                {"fr","Transfert de fichiers"},
                {"zh-chs","文件传输"},
                {"fi","Tiedostonsiirto"},
                {"tr","Dosya transferi"},
                {"cs","Přenos souboru"},
                {"ja","ファイル転送"},
                {"es","Transferencia de Archivos"},
                {"pl","Transfer Pliku"},
                {"pt","Transferência de arquivo"},
                {"nl","Bestandsoverdracht"},
                {"pt-br","Transferência de arquivo"},
                {"sv","Filöverföring"},
                {"da","Filoverførsel"},
                {"ko","파일 전송"},
                {"it","Trasferimento di file"},
                {"ru","Передача файлов"}
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
                {"pl","Zapytaj o zgodę + Bar"},
                {"pt","Peça Consentimento + Bar"},
                {"nl","Vraag toestemming + informatiebalk"},
                {"pt-br","Peça Autorização + Bar"},
                {"sv","Fråga samtycke + bar"},
                {"da","Spørg om samtykke + bar"},
                {"ko","연결 요청 + Bar"},
                {"it","Chiedi Consenso + Bar"},
                {"ru","Спросите согласия + бар"}
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
                {"pl","Stan"},
                {"pt","Estado"},
                {"nl","Status"},
                {"pt-br","Estado"},
                {"sv","stat"},
                {"da","Tilstand"},
                {"ko","상태"},
                {"it","Stato"},
                {"ru","Состояние"}
            }
        },
        {
            "Forward all keyboard keys",
            new Dictionary<string, string>() {
                {"de","Alle Tastaturtasten weiterleiten"},
                {"hi","सभी कीबोर्ड कुंजियों को अग्रेषित करें"},
                {"fr","Transférer toutes les touches du clavier"},
                {"zh-chs","转发所有键盘键"},
                {"fi","Lähetä kaikki näppäimistön näppäimet eteenpäin"},
                {"tr","Tüm klavye tuşlarını ilet"},
                {"cs","Přeposlat všechny klávesy na klávesnici"},
                {"ja","すべてのキーボードキーを転送する"},
                {"es","Reenviar todas las teclas del teclado"},
                {"pl","Przekaż wszystkie klawisze klawiatury"},
                {"pt","Encaminhar todas as teclas do teclado"},
                {"nl","Alle toetsenbordtoetsen doorsturen"},
                {"pt-br","Encaminhar todas as teclas do teclado"},
                {"sv","Vidarebefordra alla tangentbordstangenter"},
                {"da","Videresend alle tastatur taster"},
                {"ko","모든 키보드 키 전달"},
                {"it","Inoltra tutti i tasti della tastiera"},
                {"ru","Переслать все клавиши клавиатуры"}
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
                {"pl","Statystyki"},
                {"pt","Estatísticas"},
                {"nl","Statistieken"},
                {"pt-br","Estatísticas"},
                {"sv","Statistik"},
                {"da","Statistik"},
                {"ko","통계"},
                {"ru","Статистика"}
            }
        },
        {
            "Error Message",
            new Dictionary<string, string>() {
                {"de","Fehlermeldung"},
                {"hi","त्रुटि संदेश"},
                {"fr","Message d'erreur"},
                {"zh-chs","错误信息"},
                {"fi","Virheviesti"},
                {"tr","Hata mesajı"},
                {"cs","Chybové hlášení"},
                {"ja","エラーメッセージ"},
                {"es","Mensaje de error"},
                {"pl","Komunikat Błędu"},
                {"pt","Mensagem de erro"},
                {"nl","Foutmelding"},
                {"pt-br","Mensagem de erro"},
                {"sv","Felmeddelande"},
                {"da","Fejlmeddelelse"},
                {"ko","에러 메시지"},
                {"it","Messaggio di errore"},
                {"ru","Сообщение об ошибке"}
            }
        },
        {
            "Device Status",
            new Dictionary<string, string>() {
                {"de","Gerätestatus"},
                {"hi","उपकरण की स्थिति"},
                {"fr","Statut du périphérique"},
                {"zh-chs","设备状态"},
                {"fi","Laitteen tila"},
                {"tr","Cihaz durumu"},
                {"cs","Stav zařízení"},
                {"ja","デバイスステータス"},
                {"es","Estado del dispositivo"},
                {"pl","Status Urządzenia"},
                {"pt","Status do dispositivo"},
                {"nl","Apparaatstatus"},
                {"pt-br","Status do dispositivo"},
                {"sv","Enhets status"},
                {"da","Enheds status"},
                {"ko","장치 상태"},
                {"it","Stato del dispositivo"},
                {"ru","Состояние устройства"}
            }
        },
        {
            "{0} seconds left",
            new Dictionary<string, string>() {
                {"de","noch {0} Sekunden"},
                {"hi","{0} सेकंड शेष"},
                {"fr","{0} secondes restantes"},
                {"zh-chs","还剩 {0} 秒"},
                {"fi","{0} sekuntia jäljellä"},
                {"tr","{0} saniye kaldı"},
                {"cs","Zbývá {0} sekund"},
                {"ja","残り{0}秒"},
                {"es","Quedan {0} segundos"},
                {"pl","Pozostało {0} sekund"},
                {"pt","Faltam {0} segundos"},
                {"nl","{0} seconden resterend"},
                {"pt-br","Quedan {0} segundos"},
                {"sv","{0} sekunder kvar"},
                {"da","{0} sekunder tilbage"},
                {"ko","{0}초 남음"},
                {"it","{0} secondi rimasti"},
                {"ru","Осталось {0} секунд"}
            }
        },
        {
            "Routing Stats",
            new Dictionary<string, string>() {
                {"de","Routing-Statistiken"},
                {"hi","रूटिंग आँकड़े"},
                {"fr","Statistiques de routage"},
                {"zh-chs","路由统计"},
                {"fi","Reititilastot"},
                {"tr","Yönlendirme İstatistikleri"},
                {"cs","Směrovací statistiky"},
                {"ja","ルーティング統計"},
                {"es","Estadísticas de Enrutamiento"},
                {"pl","Statystyki Routingu"},
                {"pt","Estatísticas de roteamento"},
                {"nl","Routeringsstatistieken"},
                {"pt-br","Estatísticas de roteamento"},
                {"ko","라우팅 통계"},
                {"it","Statistiche di instradamento"},
                {"ru","Статистика маршрутизации"}
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
                {"pl","Zapytaj o zgodę"},
                {"pt","Peça consentimento"},
                {"nl","Vraag toestemming"},
                {"pt-br","Peça autorização"},
                {"sv","Fråga samtycke"},
                {"da","Spørg om samtykke"},
                {"ko","연결 요청"},
                {"it","Chiedi il consenso"},
                {"ru","Спросите согласия"}
            }
        },
        {
            "Wake Up...",
            new Dictionary<string, string>() {
                {"de","Wach auf..."},
                {"hi","जगाना..."},
                {"fr","Reveiller..."},
                {"zh-chs","醒来..."},
                {"fi","herätä..."},
                {"tr","uyanmak..."},
                {"cs","vzbudit..."},
                {"ja","起きろ..."},
                {"es","despertar..."},
                {"pl","budzić się..."},
                {"pt","acordar..."},
                {"nl","word wakker..."},
                {"pt-br","acordar..."},
                {"sv","vakna..."},
                {"da","Vågn op..."},
                {"ko","깨우다..."},
                {"it","svegliati..."},
                {"ru","просыпайся..."}
            }
        },
        {
            "Remote Desktop...",
            new Dictionary<string, string>() {
                {"de","Remotedesktop..."},
                {"hi","रिमोट डेस्कटॉप..."},
                {"fr","Bureau à distance..."},
                {"zh-chs","远程桌面..."},
                {"fi","Etätyöpöytä..."},
                {"tr","Uzak Masaüstü..."},
                {"cs","Vzdálená plocha..."},
                {"ja","リモートデスクトップ..."},
                {"es","Escritorio Remoto..."},
                {"pl","Zdalny Pulpit..."},
                {"pt","Área de trabalho remota..."},
                {"nl","Extern bureaublad..."},
                {"pt-br","Área de trabalho remota..."},
                {"sv","Fjärrskrivbord..."},
                {"da","Fjernskrivebord..."},
                {"ko","원격 데스크탑..."},
                {"it","Desktop remoto..."},
                {"ru","Удаленного рабочего стола..."}
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
                {"pl","Urządzenia"},
                {"pt","Dispositivos"},
                {"nl","Apparaten"},
                {"pt-br","Dispositivos"},
                {"sv","Enheter"},
                {"da","Enheder"},
                {"ko","여러 장치"},
                {"it","Dispositivi"},
                {"ru","Устройства"}
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
                {"tr","Yeniden adlandır"},
                {"cs","Přejmenovat"},
                {"ja","リネーム"},
                {"es","Renombrar"},
                {"pl","Zmień nazwę"},
                {"pt","Renomear"},
                {"nl","Hernoemen"},
                {"pt-br","Renomear"},
                {"sv","Döp om"},
                {"da","Omdøb"},
                {"ko","이름 바꾸기"},
                {"it","Rinominare"},
                {"ru","Переименовать"}
            }
        },
        {
            "0 Bytes",
            new Dictionary<string, string>() {
                {"de","0 Byte"},
                {"hi","0 बाइट्स"},
                {"fr","0 octet"},
                {"zh-chs","0 字节"},
                {"fi","0 tavua"},
                {"tr","0 Bayt"},
                {"cs","0 bajtů"},
                {"ja","0バイト"},
                {"es","0 bytes"},
                {"pl","0 Bajtów"},
                {"sv","0 byte"},
                {"da","0 bytes"},
                {"ko","0 바이트"},
                {"ru","0 байт"}
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
                {"pl","Rozłączony"},
                {"pt","Desconectado"},
                {"nl","Verbroken"},
                {"pt-br","Desconectado"},
                {"sv","Frånkopplad"},
                {"da","Afbrudt"},
                {"ko","연결 해제"},
                {"it","Disconnesso"},
                {"ru","Отключен"}
            }
        },
        {
            "E&xit",
            new Dictionary<string, string>() {
                {"de","Beenden"},
                {"hi","बाहर जाएं"},
                {"fr","Sortir"},
                {"zh-chs","出口"},
                {"fi","E & xit"},
                {"tr","Çıkış"},
                {"cs","Výstup"},
                {"ja","出口"},
                {"es","Salida"},
                {"pl","Wyjdź (&x)"},
                {"pt","Saída"},
                {"nl","Sluiten"},
                {"pt-br","Saída"},
                {"sv","Utgång"},
                {"ko","출구"},
                {"ru","Выход"}
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
                {"pl","Zdalny Pulpit"},
                {"pt","Área de trabalho remota"},
                {"nl","Extern bureaublad"},
                {"pt-br","Área de trabalho remota"},
                {"sv","Fjärrskrivbord"},
                {"da","Fjernskrivebord"},
                {"ko","원격 데스크탑"},
                {"it","Desktop remoto"},
                {"ru","Удаленного рабочего стола"}
            }
        },
        {
            "View Certificate Details...",
            new Dictionary<string, string>() {
                {"de","Zertifikatdetails anzeigen..."},
                {"hi","प्रमाणपत्र विवरण देखें..."},
                {"fr","Afficher les détails du certificat..."},
                {"zh-chs","查看证书详细信息..."},
                {"fi","Näytä varmenteen tiedot ..."},
                {"tr","Sertifika Ayrıntılarını Görüntüle..."},
                {"cs","Zobrazit podrobnosti o certifikátu ..."},
                {"ja","証明書の詳細を表示..."},
                {"es","Ver Detalles del Certificado ..."},
                {"pl","Pokaż Szczegóły Certyfikatu..."},
                {"pt","Exibir detalhes do certificado ..."},
                {"nl","Certificaatdetails bekijken..."},
                {"pt-br","Exibir detalhes do certificado ..."},
                {"sv","Visa certifikatinformation ..."},
                {"da","Vis certifikatinformation ..."},
                {"ko","인증서 세부 정보보기 ..."},
                {"it","Visualizza dettagli certificato..."},
                {"ru","Просмотреть сведения о сертификате ..."}
            }
        },
        {
            "Routing Status",
            new Dictionary<string, string>() {
                {"de","Routing-Status"},
                {"hi","रूटिंग स्थिति"},
                {"fr","État du routage"},
                {"zh-chs","路由状态"},
                {"fi","Reitityksen tila"},
                {"tr","Yönlendirme Durumu"},
                {"cs","Stav směrování"},
                {"ja","ルーティングステータス"},
                {"es","Estado de Enrutamiento"},
                {"pl","Status routingu"},
                {"pt","Status de roteamento"},
                {"nl","Routeringsstatus"},
                {"pt-br","Status de roteamento"},
                {"sv","Ruttstatus"},
                {"ko","라우팅 상태"},
                {"it","Stato del percorso"},
                {"ru","Статус маршрутизации"}
            }
        },
        {
            ", {0} connections.",
            new Dictionary<string, string>() {
                {"de",", {0} Verbindungen."},
                {"hi",", {0} कनेक्शन।"},
                {"fr",", {0} connexions."},
                {"zh-chs",", {0} 个连接。"},
                {"fi",", {0} yhteyttä."},
                {"tr",", {0} bağlantı."},
                {"cs",", {0} připojení."},
                {"ja","、{0}接続。"},
                {"es",", {0} conexiones."},
                {"pl",", {0} połączeń."},
                {"pt",", {0} conexões."},
                {"nl",", {0} verbindingen."},
                {"pt-br",", {0} conexões."},
                {"sv",", {0} anslutningar."},
                {"da",", {0} forbindelser."},
                {"ko",", {0} 명 연결."},
                {"it",", {0} connessioni."},
                {"ru",", Подключений: {0}."}
            }
        },
        {
            "{0} Gbytes/sec",
            new Dictionary<string, string>() {
                {"de","{0} Gbyte/s"},
                {"hi","{0} गीबाइट्स/सेकंड"},
                {"zh-chs","{0} GB/秒"},
                {"fi","{0} Tavu/s"},
                {"tr","{0} Gbayt/sn"},
                {"cs","{0} Gb/s"},
                {"ja","{0}ギガバイト/秒"},
                {"es","{0} Gbytes / seg"},
                {"pl","{0} GB/s"},
                {"pt","{0} Gbytes / s"},
                {"pt-br","{0} Gbytes / seg"},
                {"sv","{0} Gbyte/sek"},
                {"da","{0} Gbytes/sek"},
                {"ko","{0}GB/초"},
                {"it","{0} Gbyte/sec"},
                {"ru","{0} Гбайт / с"}
            }
        },
        {
            "{0} Mbytes/sec",
            new Dictionary<string, string>() {
                {"de","{0} Mbyte/s"},
                {"hi","{0}एमबाइट्स/सेकंड"},
                {"zh-chs","{0} 兆字节/秒"},
                {"fi","{0} Megatavua/sek"},
                {"tr","{0} Mbayt/sn"},
                {"cs","{0} MB/s"},
                {"ja","{0}メガバイト/秒"},
                {"es","{0} Mbytes / seg"},
                {"pl","{0} MB/s"},
                {"pt","{0} Mbytes / s"},
                {"pt-br","{0} Mbytes / seg"},
                {"sv","{0} Mbytes/sek"},
                {"da","{0} Mbytes/sek"},
                {"ko","{0}MB/초"},
                {"it","{0} Mbyte/sec"},
                {"ru","{0} Мбайт / с"}
            }
        },
        {
            "Stopped.",
            new Dictionary<string, string>() {
                {"de","Gestoppt."},
                {"hi","रोका हुआ।"},
                {"fr","Arrêté."},
                {"zh-chs","停了。"},
                {"fi","Pysähtyi."},
                {"tr","Durdu."},
                {"cs","Zastavil."},
                {"ja","停止。"},
                {"es","Detenido."},
                {"pl","Zatrzymany."},
                {"pt","Parado."},
                {"nl","Gestopt."},
                {"pt-br","Parado."},
                {"sv","Stannade."},
                {"da","Stoppet."},
                {"ko","중지되었습니다."},
                {"ru","Остановился."}
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
                {"pl","Operacja na Pliku"},
                {"pt","Operação de arquivo"},
                {"nl","Bestandsbewerking"},
                {"pt-br","Operação de arquivo"},
                {"sv","Filhantering"},
                {"da","Filhåndtering"},
                {"ko","파일 작업"},
                {"it","Operazione sui file"},
                {"ru","Работа с файлами"}
            }
        },
        {
            "{0} Bytes",
            new Dictionary<string, string>() {
                {"de","{0} Byte"},
                {"hi","{0} बाइट्स"},
                {"fr","{0} octets"},
                {"zh-chs","{0} 字节"},
                {"fi","{0} Tavua"},
                {"tr","{0} Bayt"},
                {"cs","{0} bajtů"},
                {"ja","{0}バイト"},
                {"es","{0} bytes"},
                {"pl","{0} Bajtów"},
                {"pt","{0} bytes"},
                {"pt-br","{0} bytes"},
                {"sv","{0} Byte"},
                {"ko","{0} 바이트"},
                {"ru","{0} байтов"}
            }
        },
        {
            "Stopped",
            new Dictionary<string, string>() {
                {"de","Gestoppt"},
                {"hi","रोका हुआ"},
                {"fr","Arrêté"},
                {"zh-chs","停止"},
                {"fi","Pysähtyi"},
                {"tr","durduruldu"},
                {"cs","Zastavil"},
                {"ja","停止"},
                {"es","Detenido"},
                {"pl","Zatrzymany"},
                {"pt","Parado"},
                {"nl","Gestopt"},
                {"pt-br","Parado"},
                {"sv","Stannade"},
                {"da","Stoppet"},
                {"ko","중지됨"},
                {"it","Fermato"},
                {"ru","Остановлен"}
            }
        },
        {
            "Mappings",
            new Dictionary<string, string>() {
                {"de","Zuordnungen"},
                {"hi","मानचित्रण"},
                {"fr","Mappages"},
                {"zh-chs","映射"},
                {"fi","Kartoitukset"},
                {"tr","Eşlemeler"},
                {"cs","Mapování"},
                {"ja","マッピング"},
                {"es","Mapeos"},
                {"pl","Mapowanie"},
                {"pt","Mapeamentos"},
                {"nl","Toewijzingen"},
                {"pt-br","Mapeamentos"},
                {"sv","Kartläggningar"},
                {"da","Kortlægninger"},
                {"ko","매핑"},
                {"it","Mappature"},
                {"ru","Сопоставления"}
            }
        },
        {
            "Don't ask for {0} days.",
            new Dictionary<string, string>() {
                {"de","Frag nicht nach {0} Tagen."},
                {"hi","{0} दिनों के लिए मत पूछो।"},
                {"fr","Ne demandez pas {0} jours."},
                {"zh-chs","不要要求 {0} 天。"},
                {"fi","Älä kysy {0} päivää."},
                {"tr","{0} gün sormayın."},
                {"cs","Nepožadujte {0} dny."},
                {"ja","{0}日を要求しないでください。"},
                {"es","No pedir por {0} días."},
                {"pl","Nie pytaj przez {0} dni."},
                {"pt","Não pergunte por {0} dias."},
                {"nl","Niet vragen voor {0} dagen."},
                {"pt-br","Não pergunte por {0} dias."},
                {"sv","Be inte om {0} dagar."},
                {"da","Spørg ikke i {0} dage."},
                {"ko","{0} 일을 요구하지 마세요."},
                {"it","Non chiedere per  {0} giorni."},
                {"ru","Не просите {0} дней."}
            }
        },
        {
            "Remove 1 item?",
            new Dictionary<string, string>() {
                {"de","1 Artikel entfernen?"},
                {"hi","1 आइटम निकालें?"},
                {"fr","Supprimer 1 élément ?"},
                {"zh-chs","删除 1 项？"},
                {"fi","Poistetaanko 1 kohde?"},
                {"tr","1 öğe kaldırılsın mı?"},
                {"cs","Odebrat 1 položku?"},
                {"ja","1つのアイテムを削除しますか？"},
                {"es","¿Eliminar 1 artículo?"},
                {"pl","Usunąć 1 pozycję?"},
                {"pt","Remover 1 item?"},
                {"nl","1 artikel verwijderen?"},
                {"pt-br","Remover 1 item?"},
                {"sv","Ta bort ett objekt?"},
                {"da","Fjern 1 element?"},
                {"ko","항목 1 개를 삭제 하시겠습니까?"},
                {"it","Rimuovere 1 elemento?"},
                {"ru","Удалить 1 элемент?"}
            }
        },
        {
            "Remote Device",
            new Dictionary<string, string>() {
                {"de","Remote-Gerät"},
                {"hi","रिमोट डिवाइस"},
                {"fr","Périphérique distant"},
                {"zh-chs","遥控装置"},
                {"fi","Etälaite"},
                {"tr","Uzak Cihaz"},
                {"cs","Vzdálené zařízení"},
                {"ja","リモートデバイス"},
                {"es","Dispositivo Remoto"},
                {"pl","Urządzenie Zdalne"},
                {"pt","Dispositivo Remoto"},
                {"nl","Extern apparaat"},
                {"pt-br","Dispositivo Remoto"},
                {"sv","Fjärrenhet"},
                {"da","Fjernenhed"},
                {"ko","원격 장치"},
                {"it","Dispositivo remoto"},
                {"ru","Удаленное устройство"}
            }
        },
        {
            "Remote desktop quality, scaling and frame rate settings. These can be adjusted depending on the quality of the network connection.",
            new Dictionary<string, string>() {
                {"de","Remote-Desktop-Qualität, Skalierung und Bildrateneinstellungen. Diese können je nach Qualität der Netzwerkverbindung angepasst werden."},
                {"hi","दूरस्थ डेस्कटॉप गुणवत्ता, स्केलिंग और फ्रेम दर सेटिंग्स। इन्हें नेटवर्क कनेक्शन की गुणवत्ता के आधार पर समायोजित किया जा सकता है।"},
                {"fr","Paramètres de qualité, de mise à l'échelle et de fréquence d'images du bureau à distance. Ceux-ci peuvent être ajustés en fonction de la qualité de la connexion réseau."},
                {"zh-chs","远程桌面质量、缩放和帧速率设置。这些可以根据网络连接的质量进行调整。"},
                {"fi","Etätyöpöydän laatu-, skaalaus- ja kuvataajuusasetukset. Näitä voidaan säätää verkkoyhteyden laadun mukaan."},
                {"tr","Uzak masaüstü kalitesi, ölçekleme ve kare hızı ayarları. Bunlar, ağ bağlantısının kalitesine bağlı olarak ayarlanabilir."},
                {"cs","Kvalita vzdálené plochy, měřítko a nastavení snímkové frekvence. Ty lze upravit v závislosti na kvalitě síťového připojení."},
                {"ja","リモートデスクトップの品質、スケーリング、フレームレートの設定。これらは、ネットワーク接続の品質に応じて調整できます。"},
                {"es","Configuración de la calidad del escritorio remoto, la escala y la velocidad de fotogramas. Estos se pueden ajustar en función de la calidad de la conexión de red."},
                {"pl","Ustawienia jakości pulpitu zdalnego, skalowania i częstotliwości odświeżania. Można je dostosować w zależności od jakości połączenia sieciowego."},
                {"pt","Configurações de qualidade, escala e taxa de quadros da área de trabalho remota. Eles podem ser ajustados dependendo da qualidade da conexão de rede."},
                {"nl","Instellingen voor kwaliteit, schaal en framesnelheid van het externe bureaublad. Deze kunnen worden aangepast afhankelijk van de kwaliteit van de netwerkverbinding."},
                {"pt-br","Configurações de qualidade, escala e taxa de quadros da área de trabalho remota. Eles podem ser ajustados dependendo da qualidade da conexão de rede."},
                {"sv","Fjärrskrivbords kvalitet, skalning och bildfrekvensinställningar. Dessa kan justeras beroende på nätverksanslutningens kvalitet."},
                {"da","Indstillinger for fjernskrivebordskvalitet, skalering og billedhastighed. Disse kan justeres afhængigt af kvaliteten af netværksforbindelsen."},
                {"ko","원격 데스크톱 품질, 크기 조정 및 프레임 속도 설정. 네트워크 연결 품질에 따라 조정할 수 있습니다."},
                {"it","Impostazioni di qualità, ridimensionamento e frame rate del desktop remoto. Questi possono essere regolati in base alla qualità della connessione di rete."},
                {"ru","Настройки качества, масштабирования и частоты кадров удаленного рабочего стола. Их можно настроить в зависимости от качества сетевого подключения."}
            }
        },
        {
            "Local Port",
            new Dictionary<string, string>() {
                {"de","Lokaler Port"},
                {"hi","स्थानीय बंदरगाह"},
                {"fr","Port local"},
                {"zh-chs","本地端口"},
                {"fi","Paikallinen satama"},
                {"tr","Yerel Liman"},
                {"cs","Místní přístav"},
                {"ja","ローカルポート"},
                {"es","Puerto Local"},
                {"pl","Port Lokalny"},
                {"pt","Porto Local"},
                {"nl","Lokale poort"},
                {"pt-br","Porta Local"},
                {"sv","Lokal hamn"},
                {"da","Lokal Port"},
                {"ko","로컬 포트"},
                {"it","Porta locale"},
                {"ru","Местный порт"}
            }
        },
        {
            "Stats...",
            new Dictionary<string, string>() {
                {"de","Statistiken..."},
                {"hi","आँकड़े..."},
                {"fr","Statistiques..."},
                {"zh-chs","统计..."},
                {"fi","Tilastot ..."},
                {"tr","İstatistikler..."},
                {"cs","Statistiky ..."},
                {"ja","統計..."},
                {"es","Estadísticas ..."},
                {"pl","Statystyki..."},
                {"pt","Estatísticas..."},
                {"nl","Statistieken..."},
                {"pt-br","Estatísticas..."},
                {"sv","Statistik ..."},
                {"da","Statistik ..."},
                {"ko","통계 ..."},
                {"it","Statistiche..."},
                {"ru","Статистика ..."}
            }
        },
        {
            "This MeshCentral Server uses a different version of this tool. Click ok to download and update.",
            new Dictionary<string, string>() {
                {"de","Dieser MeshCentral Server verwendet eine andere Version dieses Tools. Klicken Sie auf OK, um herunterzuladen und zu aktualisieren."},
                {"hi","यह MeshCentral सर्वर इस टूल के भिन्न संस्करण का उपयोग करता है। डाउनलोड और अपडेट करने के लिए ओके पर क्लिक करें।"},
                {"fr","Ce serveur MeshCentral utilise une version différente de cet outil. Cliquez sur ok pour télécharger et mettre à jour."},
                {"zh-chs","此 MeshCentral Server 使用此工具的不同版本。单击“确定”进行下载和更新。"},
                {"fi","Tämä MeshCentral -palvelin käyttää eri versiota tästä työkalusta. Lataa ja päivitä napsauttamalla OK."},
                {"tr","Bu MeshCentral Sunucusu, bu aracın farklı bir sürümünü kullanır. İndirmek ve güncellemek için Tamam'a tıklayın."},
                {"cs","Tento server MeshCentral používá jinou verzi tohoto nástroje. Kliknutím na ok stáhnete a aktualizujete."},
                {"ja","このMeshCentralサーバーは、このツールの異なるバージョンを使用します。 [OK]をクリックして、ダウンロードして更新します。"},
                {"es","Este servidor MeshCentral utiliza una versión diferente de esta herramienta. Haz clic en Aceptar para descargar y actualizar."},
                {"pl","Ten MeshCentral Server używa innej wersji tego narzędzia. Kliknij ok, aby pobrać i zaktualizować."},
                {"pt","Este servidor MeshCentral usa uma versão diferente desta ferramenta. Clique em ok para baixar e atualizar."},
                {"nl","Deze MeshCentral Server gebruikt een andere versie van deze tool. Klik op OK om te downloaden en bij te werken."},
                {"pt-br","Este servidor MeshCentral usa uma versão diferente desta ferramenta. Clique em ok para baixar e atualizar."},
                {"sv","Denna MeshCentral Server använder en annan version av detta verktyg. Klicka på ok för att ladda ner och uppdatera."},
                {"da","Denne MeshCentral Server bruger en anden version af dette værktøj. Klik på ok for at downloade og opdatere."},
                {"ko","이 MeshCentral 서버는이 도구의 다른 버전을 사용합니다. 확인을 클릭하여 다운로드하고 업데이트하십시오."},
                {"it","Questo server MeshCentral utilizza una versione diversa di questo strumento.Fare clic su OK per scaricare e aggiornare."},
                {"ru","Этот MeshCentral Server использует другую версию этого инструмента. Нажмите ОК, чтобы загрузить и обновить."}
            }
        },
        {
            "Next",
            new Dictionary<string, string>() {
                {"de","Nächster"},
                {"hi","अगला"},
                {"fr","Suivant"},
                {"zh-chs","下一个"},
                {"fi","Seuraava"},
                {"tr","Sonraki"},
                {"cs","další"},
                {"ja","次"},
                {"es","Próximo"},
                {"pl","Dalej"},
                {"pt","Próximo"},
                {"nl","Volgende"},
                {"pt-br","Próximo"},
                {"sv","Nästa"},
                {"da","Næste"},
                {"ko","다음"},
                {"it","Prossimo"},
                {"ru","Следующий"}
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
                {"pl","E-mail"},
                {"nl","E-mail"},
                {"pt-br","E-mail"},
                {"sv","E-post"},
                {"da","E-mail"},
                {"ko","이메일"},
                {"it","E-mail"}
            }
        },
        {
            "WARNING - Invalid Server Certificate",
            new Dictionary<string, string>() {
                {"de","WARNUNG - Ungültiges Serverzertifikat"},
                {"hi","चेतावनी - अमान्य सर्वर प्रमाणपत्र"},
                {"fr","AVERTISSEMENT - Certificat de serveur non valide"},
                {"zh-chs","警告 - 服务器证书无效"},
                {"fi","VAROITUS - Virheellinen palvelinvarmenne"},
                {"tr","UYARI - Geçersiz Sunucu Sertifikası"},
                {"cs","UPOZORNĚNÍ - Neplatný certifikát serveru"},
                {"ja","警告-無効なサーバー証明書"},
                {"es","ADVERTENCIA: Certificado de Servidor No Válido"},
                {"pl","OSTRZEŻENIE - Nieprawidłowy certyfikat serwera"},
                {"pt","AVISO - Certificado de servidor inválido"},
                {"nl","WAARSCHUWING - Ongeldig servercertificaat"},
                {"pt-br","AVISO - Certificado de servidor inválido"},
                {"sv","VARNING - Ogiltigt servercertifikat"},
                {"da","ADVARSEL - Ugyldigt servercertifikat"},
                {"ko","경고-잘못된 서버 인증서"},
                {"it","AVVISO - Certificato del server non valido"},
                {"ru","ВНИМАНИЕ! Недействительный сертификат сервера."}
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
                {"pl","Nazwa użytkownika"},
                {"pt","Nome de usuário"},
                {"nl","Gebruikersnaam"},
                {"pt-br","Nome do usuário"},
                {"sv","Användarnamn"},
                {"da","Brugernavn"},
                {"ko","사용자 이름"},
                {"it","Nome utente"},
                {"ru","Имя пользователя"}
            }
        },
        {
            "Change remote desktop settings",
            new Dictionary<string, string>() {
                {"de","Remote-Desktop-Einstellungen ändern"},
                {"hi","दूरस्थ डेस्कटॉप सेटिंग बदलें"},
                {"fr","Modifier les paramètres du bureau à distance"},
                {"zh-chs","更改远程桌面设置"},
                {"fi","Muuta etätyöpöydän asetuksia"},
                {"tr","Uzak masaüstü ayarlarını değiştir"},
                {"cs","Změňte nastavení vzdálené plochy"},
                {"ja","リモートデスクトップ設定を変更する"},
                {"es","Cambiar la configuración del escritorio remoto"},
                {"pl","Zmień ustawienia pulpitu zdalnego"},
                {"pt","Alterar as configurações da área de trabalho remota"},
                {"nl","Instellingen voor extern bureaublad wijzigen"},
                {"pt-br","Alterar as configurações da área de trabalho remota"},
                {"sv","Ändra fjärrskrivbordsinställningar"},
                {"da","Skift indstillinger for fjernskrivebord"},
                {"ko","원격 데스크톱 설정 변경"},
                {"it","Modifica le impostazioni del desktop remoto"},
                {"ru","Изменить настройки удаленного рабочего стола"}
            }
        },
        {
            "Enter the RDP port of the remote computer, the default RDP port is 3389.",
            new Dictionary<string, string>() {
                {"de","Geben Sie den RDP-Port des Remote-Computers ein, der Standard-RDP-Port ist 3389."},
                {"hi","दूरस्थ कंप्यूटर का RDP पोर्ट दर्ज करें, डिफ़ॉल्ट RDP पोर्ट 3389 है।"},
                {"fr","Entrez le port RDP de l'ordinateur distant, le port RDP par défaut est 3389."},
                {"zh-chs","输入远程计算机的RDP端口，默认RDP端口为3389。"},
                {"fi","Anna etätietokoneen RDP -portti, oletusarvoinen RDP -portti on 3389."},
                {"tr","Uzak bilgisayarın RDP bağlantı noktasını girin, varsayılan RDP bağlantı noktası 3389'dur."},
                {"cs","Zadejte port RDP vzdáleného počítače, výchozí port RDP je 3389."},
                {"ja","リモートコンピューターのRDPポートを入力します。デフォルトのRDPポートは3389です。"},
                {"es","Ingresa el puerto RDP de la computadora remota, el puerto RDP predeterminado es 3389."},
                {"pl","Wprowadź port RDP komputera zdalnego, domyślny port RDP to 3389."},
                {"pt","Insira a porta RDP do computador remoto; a porta RDP padrão é 3389."},
                {"nl","Voer de RDP poort van de externe computer in, de standaard RDP poort is 3389."},
                {"pt-br","Insira a porta RDP do computador remoto, a porta RDP padrão é 3389."},
                {"sv","Ange RDP-porten på fjärrdatorn, standard-RDP-porten är 3389."},
                {"da","Indtast RDP-porten på fjerncomputeren, standard RDP-porten er 3389."},
                {"ko","원격 컴퓨터의 RDP 포트를 입력합니다. 기본 RDP 포트는 3389입니다."},
                {"it","Immettere la porta RDP del computer remoto, la porta RDP predefinita è 3389."},
                {"ru","Введите порт RDP удаленного компьютера, порт RDP по умолчанию - 3389."}
            }
        },
        {
            "Device Settings",
            new Dictionary<string, string>() {
                {"de","Geräteeinstellungen"},
                {"hi","उपकरण सेटिंग्स"},
                {"fr","Réglages de l'appareil"},
                {"zh-chs","设备设置"},
                {"fi","Laite asetukset"},
                {"tr","Cihaz ayarları"},
                {"cs","Nastavení zařízení"},
                {"ja","デバイスの設定"},
                {"es","Configuración de dispositivo"},
                {"pl","Ustawienia Urządzenia"},
                {"pt","Configurações do dispositivo"},
                {"nl","Apparaat instellingen"},
                {"pt-br","Configurações do dispositivo"},
                {"sv","Enhetsinställningar"},
                {"da","Enhedsindstillinger"},
                {"ko","기기 설정"},
                {"it","Impostazioni del dispositivo"},
                {"ru","Настройки устройства"}
            }
        },
        {
            "Open Source, Apache 2.0 License",
            new Dictionary<string, string>() {
                {"de","Open Source, Apache 2.0-Lizenz"},
                {"hi","ओपन सोर्स, अपाचे 2.0 लाइसेंस"},
                {"fr","Open Source, licence Apache 2.0"},
                {"zh-chs","开源，Apache 2.0 许可"},
                {"fi","Avoimen lähdekoodin Apache 2.0 -lisenssi"},
                {"tr","Açık Kaynak, Apache 2.0 Lisansı"},
                {"cs","Open Source, licence Apache 2.0"},
                {"ja","オープンソース、Apache2.0ライセンス"},
                {"es","Código Abierto, licencia Apache 2.0"},
                {"pt","Código aberto, licença Apache 2.0"},
                {"nl","Open Source, Apache 2.0 Licentie"},
                {"pt-br","Código aberto, licença Apache 2.0"},
                {"sv","Open Source, Apache 2.0-licens"},
                {"da","Open Source, Apache 2.0-licens"},
                {"ko","오픈 소스, Apache 2.0 라이선스"},
                {"it","Open Source, licenza Apache 2.0"},
                {"ru","Открытый исходный код, лицензия Apache 2.0"}
            }
        },
        {
            "Connection",
            new Dictionary<string, string>() {
                {"de","Verbindung"},
                {"hi","संबंध"},
                {"fr","Lien"},
                {"zh-chs","联系"},
                {"fi","Yhteys"},
                {"tr","Bağlantı"},
                {"cs","Spojení"},
                {"ja","繋がり"},
                {"es","Conexión"},
                {"pl","Połączenie"},
                {"pt","Conexão"},
                {"nl","Verbinding"},
                {"pt-br","Conexão"},
                {"sv","Förbindelse"},
                {"da","Forbindelse"},
                {"ko","연결"},
                {"it","Connessione"},
                {"ru","Связь"}
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
                {"pl","Lokalny"},
                {"nl","Lokaal"},
                {"sv","Lokal"},
                {"da","Lokal"},
                {"ko","로컬"},
                {"it","Locale"},
                {"ru","Локальный"}
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
                {"pl","Protokół"},
                {"pt","Protocolo"},
                {"pt-br","Protocolo"},
                {"sv","Protokoll"},
                {"ko","프로토콜"},
                {"it","Protocollo"},
                {"ru","Протокол"}
            }
        },
        {
            "Transfer Progress",
            new Dictionary<string, string>() {
                {"de","Übertragungsfortschritt"},
                {"hi","स्थानांतरण प्रगति"},
                {"fr","Progression du transfert"},
                {"zh-chs","转学进度"},
                {"fi","Siirron eteneminen"},
                {"tr","Transfer İlerlemesi"},
                {"cs","Průběh přenosu"},
                {"ja","転送の進行状況"},
                {"es","Progreso de la Transferencia"},
                {"pl","Postęp Transferu"},
                {"pt","Progresso da Transferência"},
                {"nl","Voortgang overdracht"},
                {"pt-br","Progresso da Transferência"},
                {"sv","Överför framsteg"},
                {"da","Overførselsfremgang"},
                {"ko","전송 진행"},
                {"it","Avanzamento del trasferimento"},
                {"ru","Прогресс передачи"}
            }
        },
        {
            "Complete 2FA setup online first",
            new Dictionary<string, string>() {
                {"de","Vervollständigen Sie zunächst die 2FA-Einrichtung online"},
                {"hi","पहले ऑनलाइन 2FA सेटअप पूरा करें"},
                {"fr","Terminez d'abord la configuration 2FA en ligne"},
                {"zh-chs","首先在线完成 2FA 设置"},
                {"fi","Suorita ensin 2FA-asennus verkossa"},
                {"tr","Önce çevrimiçi 2FA kurulumunu tamamlayın"},
                {"cs","Nejprve dokončete nastavení 2FA online"},
                {"ja","まずオンラインで2FAのセットアップを完了してください"},
                {"es","Complete primero la configuración de 2FA en línea"},
                {"pl","Najpierw zakończ konfigurację 2FA online"},
                {"pt","Primeiro, conclua a configuração do 2FA online"},
                {"nl","Voltooi eerst de online 2FA-instelling"},
                {"pt-br","Primeiro, conclua a configuração do 2FA online"},
                {"sv","Slutför först 2FA-inställningen online"},
                {"da","Færdiggør først 2FA-opsætningen online"},
                {"ko","먼저 온라인으로 2FA 설정을 완료하세요."},
                {"it","Completa prima la configurazione di 2FA online"},
                {"ru","Сначала завершите настройку 2FA онлайн"}
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
