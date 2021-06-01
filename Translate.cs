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
            "Count",
            new Dictionary<string, string>() {
                {"ru","Считать"},
                {"fi","Считать"},
                {"fr","Compter"},
                {"es","Contar"}
            }
        },
        {
            "Intel® ME State...",
            new Dictionary<string, string>() {
                {"ru","Состояние Intel® ME ..."},
                {"fi","Состояние Intel® ME ..."},
                {"fr","État Intel® ME..."},
                {"es","Estado Intel® ME ..."}
            }
        },
        {
            "Remote Sessions...",
            new Dictionary<string, string>() {
                {"ru","Удаленные сеансы ..."},
                {"fi","Удаленные сеансы ..."},
                {"fr","Séances à distance..."},
                {"es","Sesiones remotas ..."}
            }
        },
        {
            "Remote Sessions",
            new Dictionary<string, string>() {
                {"ru","Удаленные сеансы"},
                {"fi","Удаленные сеансы"},
                {"fr","Séances à distance"},
                {"es","Sesiones remotas"}
            }
        },
        {
            "MeshCentral Assistant",
            new Dictionary<string, string>() {
                {"fr","Assistant MeshCentral"},
                {"es","Asistente MeshCentral"},
                {"ru","MeshCentral Ассистент"}
            }
        },
        {
            "&Start Agent",
            new Dictionary<string, string>() {
                {"ru","& Запустить агент"},
                {"fi","& Запустить агент"},
                {"fr","&Démarrer l'agent"},
                {"es","& Iniciar agente"}
            }
        },
        {
            "Agent Select",
            new Dictionary<string, string>() {
                {"ru","Агент Выбрать"},
                {"fi","Агент Выбрать"},
                {"fr","Sélection d'agent"},
                {"es","Seleccionar agente"}
            }
        },
        {
            "Agent is paused",
            new Dictionary<string, string>() {
                {"ru","Агент приостановлен"},
                {"fi","Агент приостановлен"},
                {"fr","L'agent est en pause"},
                {"es","El agente está en pausa"}
            }
        },
        {
            "Agent is continue pending",
            new Dictionary<string, string>() {
                {"ru","Агент ожидает продолжения"},
                {"fi","Агент ожидает продолжения"},
                {"fr","L'agent est en attente de poursuite"},
                {"es","El agente sigue pendiente"}
            }
        },
        {
            "Later",
            new Dictionary<string, string>() {
                {"ru","Позже"},
                {"fi","Позже"},
                {"fr","Plus tard"},
                {"es","Mas tarde"}
            }
        },
        {
            "Agent is stopped pending",
            new Dictionary<string, string>() {
                {"ru","Агент остановлен в ожидании"},
                {"fi","Агент остановлен в ожидании"},
                {"fr","L'agent est arrêté en attente"},
                {"es","El agente está detenido pendiente"}
            }
        },
        {
            "Agent is missing",
            new Dictionary<string, string>() {
                {"ru","Агент отсутствует"},
                {"fi","Агент отсутствует"},
                {"fr","L'agent est manquant"},
                {"es","Falta el agente"}
            }
        },
        {
            "Intel® Management Engine state for this computer.",
            new Dictionary<string, string>() {
                {"ru","Состояние Intel® Management Engine для этого компьютера."},
                {"fi","Состояние Intel® Management Engine для этого компьютера."},
                {"fr","État du moteur de gestion Intel® pour cet ordinateur."},
                {"es","Estado del motor de administración Intel® para este equipo."}
            }
        },
        {
            "Item",
            new Dictionary<string, string>() {
                {"ru","Пункт"},
                {"fi","Пункт"},
                {"fr","Article"},
                {"es","Artículo"}
            }
        },
        {
            "Loading...",
            new Dictionary<string, string>() {
                {"de","Laden..."},
                {"hi","लोड हो रहा है..."},
                {"fr","Chargement..."},
                {"zh-cht","載入中..."},
                {"zh-chs","载入中..."},
                {"fi","Ladataan..."},
                {"tr","Yükleniyor..."},
                {"cs","Načítání…"},
                {"ja","読み込み中..."},
                {"es","Cargando..."},
                {"pt","Carregando..."},
                {"nl","Laden..."},
                {"ko","불러오는 중 ..."},
                {"ru","Загрузка..."}
            }
        },
        {
            "&Update Software",
            new Dictionary<string, string>() {
                {"ru","&Обновление программного обеспечения"},
                {"fi","&Обновление программного обеспечения"},
                {"fr","&Mettre à jour le logiciel"},
                {"es","&Actualiza el software"}
            }
        },
        {
            "Agent is disconnected",
            new Dictionary<string, string>() {
                {"ru","Агент отключен"},
                {"fi","Агент отключен"},
                {"fr","L'agent est déconnecté"},
                {"es","El agente está desconectado"}
            }
        },
        {
            "Request Help",
            new Dictionary<string, string>() {
                {"ru","Запросить помощь"},
                {"fi","Запросить помощь"},
                {"fr","Demander de l'aide"},
                {"es","Solicitar ayuda"}
            }
        },
        {
            "Remote Sessions: {0}",
            new Dictionary<string, string>() {
                {"ru","Удаленные сеансы: {0}"},
                {"fi","Удаленные сеансы: {0}"},
                {"fr","Sessions à distance : {0}"},
                {"es","Sesiones remotas: {0}"}
            }
        },
        {
            "Assistant Update",
            new Dictionary<string, string>() {
                {"ru","Ассистент Обновление"},
                {"fi","Ассистент Обновление"},
                {"fr","Mise à jour de l'assistant"},
                {"es","Asistente de actualización"}
            }
        },
        {
            "User",
            new Dictionary<string, string>() {
                {"de","Benutzer"},
                {"hi","उपयोगकर्ता"},
                {"fr","Utilisateur"},
                {"zh-cht","用戶"},
                {"zh-chs","用户"},
                {"fi","Käyttäjä"},
                {"tr","Kullanıcı"},
                {"cs","Uživatel"},
                {"ja","ユーザー"},
                {"es","Usuario"},
                {"pt","Do utilizador"},
                {"nl","Gebruiker"},
                {"ko","사용자"},
                {"ru","Пользователь"}
            }
        },
        {
            "{0} remote sessions",
            new Dictionary<string, string>() {
                {"ru","{0} удаленных сеансов"},
                {"fi","{0} удаленных сеансов"},
                {"fr","{0} sessions à distance"},
                {"es","{0} sesiones remotas"}
            }
        },
        {
            "Notify",
            new Dictionary<string, string>() {
                {"de","Benachrichtigen"},
                {"hi","सूचित करें"},
                {"fr","Notifier"},
                {"zh-cht","通知"},
                {"zh-chs","通知"},
                {"fi","Ilmoita"},
                {"tr","Bildir"},
                {"cs","Upozornit"},
                {"ja","通知する"},
                {"es","Notificar"},
                {"pt","Notificar"},
                {"nl","Melden"},
                {"ko","알림"},
                {"ru","Уведомить"}
            }
        },
        {
            "Enter help request details",
            new Dictionary<string, string>() {
                {"ru","Введите детали запроса на помощь"},
                {"fi","Введите детали запроса на помощь"},
                {"fr","Entrez les détails de la demande d'aide"},
                {"es","Ingrese los detalles de la solicitud de ayuda"}
            }
        },
        {
            "{0} Assistant",
            new Dictionary<string, string>() {
                {"ru","{0} Ассистент"},
                {"fi","{0} Ассистент"},
                {"fr","{0} Assistante"},
                {"es","{0} Asistente"}
            }
        },
        {
            "&Open",
            new Dictionary<string, string>() {
                {"ru","&Открыть"},
                {"fi","&Открыть"},
                {"fr","&Ouvert"},
                {"es","&Abierto"}
            }
        },
        {
            "{0} remote sessions are active.",
            new Dictionary<string, string>() {
                {"ru","Активных удаленных сеансов: {0}."},
                {"fi","Активных удаленных сеансов: {0}."},
                {"fr","{0} sessions à distance sont actives."},
                {"es","{0} sesiones remotas están activas."}
            }
        },
        {
            "E&xit",
            new Dictionary<string, string>() {
                {"ru","Выход"},
                {"fi","Выход"},
                {"fr","Sortir"},
                {"es","Salida"}
            }
        },
        {
            "Time",
            new Dictionary<string, string>() {
                {"de","Zeit"},
                {"hi","समय"},
                {"fr","Temps"},
                {"zh-cht","時間"},
                {"zh-chs","时间"},
                {"fi","Aika"},
                {"tr","Zaman"},
                {"cs","Čas"},
                {"ja","時間"},
                {"es","Tiempo"},
                {"pt","Tempo"},
                {"nl","Tijd"},
                {"ko","시간"},
                {"ru","Время"}
            }
        },
        {
            "Events",
            new Dictionary<string, string>() {
                {"de","Ereignisse"},
                {"hi","आयोजन"},
                {"fr","Événements"},
                {"zh-cht","事件"},
                {"zh-chs","事件"},
                {"fi","Tapahtumat"},
                {"tr","Etkinlikler"},
                {"cs","Události"},
                {"ja","イベント"},
                {"es","Eventos"},
                {"pt","Eventos"},
                {"nl","Gebeurtenissen"},
                {"ko","이벤트"},
                {"ru","События"}
            }
        },
        {
            "Versions",
            new Dictionary<string, string>() {
                {"ru","Версии "},
                {"fi","Версии"},
                {"es","Versiones"}
            }
        },
        {
            "Agent is start pending",
            new Dictionary<string, string>() {
                {"ru","Агент ожидает запуска"},
                {"fi","Агент ожидает запуска"},
                {"fr","L'agent est en attente de démarrage"},
                {"es","El agente está pendiente de inicio"}
            }
        },
        {
            "Type",
            new Dictionary<string, string>() {
                {"de","Typ"},
                {"hi","प्रकार"},
                {"zh-cht","類型"},
                {"zh-chs","类型"},
                {"fi","Tyyppi"},
                {"tr","tip"},
                {"cs","Typ"},
                {"ja","タイプ"},
                {"es","Tipo"},
                {"pt","Tipo"},
                {"ko","유형"},
                {"ru","Удаленный ввод"}
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
            "Connected to server",
            new Dictionary<string, string>() {
                {"ru","Подключено к серверу"},
                {"fi","Подключено к серверу"},
                {"fr","Connecté au serveur"},
                {"es","Conectado al servidor"}
            }
        },
        {
            "Value",
            new Dictionary<string, string>() {
                {"ru","Значение"},
                {"fi","Значение"},
                {"fr","Valeur"},
                {"es","Valor"}
            }
        },
        {
            "List of remote sessions active on this computer.",
            new Dictionary<string, string>() {
                {"ru","Список удаленных сеансов, активных на этом компьютере."},
                {"fi","Список удаленных сеансов, активных на этом компьютере."},
                {"fr","Liste des sessions distantes actives sur cet ordinateur."},
                {"es","Lista de sesiones remotas activas en esta computadora."}
            }
        },
        {
            "Help Requested",
            new Dictionary<string, string>() {
                {"ru","Запрошена помощь"},
                {"fi","Запрошена помощь"},
                {"fr","Aide demandée"},
                {"es","Ayuda solicitada"}
            }
        },
        {
            "Enabled",
            new Dictionary<string, string>() {
                {"de","aktiviert"},
                {"hi","सक्रिय"},
                {"fr","Activer"},
                {"zh-cht","已啟用"},
                {"zh-chs","已启用"},
                {"fi","Käytössä"},
                {"tr","Etkin"},
                {"cs","Povoleno"},
                {"ja","有効"},
                {"es","Habilitado"},
                {"pt","ativado"},
                {"nl","Ingeschakeld"},
                {"ko","활성화 됨"},
                {"ru","Включено"}
            }
        },
        {
            "Files",
            new Dictionary<string, string>() {
                {"de","Dateien"},
                {"hi","फ़ाइलें"},
                {"fr","Dossiers"},
                {"zh-cht","檔案"},
                {"zh-chs","档案"},
                {"fi","Tiedostot"},
                {"tr","Dosyalar"},
                {"cs","Soubory"},
                {"ja","ファイル"},
                {"es","Archivos"},
                {"pt","Arquivos"},
                {"nl","Bestanden"},
                {"ko","파일"},
                {"ru","Файлы"}
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
            "Show Sessions...",
            new Dictionary<string, string>() {
                {"ru","Показать сеансы ..."},
                {"fi","Показать сеансы ..."},
                {"fr","Afficher les séances..."},
                {"es","Mostrar sesiones ..."}
            }
        },
        {
            "PrivacyBarForm",
            new Dictionary<string, string>() {
                {"ru","Конфиденциальность"},
                {"fi","Конфиденциальность"}
            }
        },
        {
            "Agent is pause pending",
            new Dictionary<string, string>() {
                {"ru","Агент ожидает приостановки"},
                {"fi","Агент ожидает приостановки"},
                {"fr","L'agent est en attente de pause"},
                {"es","El agente está en pausa pendiente"}
            }
        },
        {
            "Agent not installed",
            new Dictionary<string, string>() {
                {"ru","Агент не установлен"},
                {"fi","Агент не установлен"},
                {"fr","Agent non installé"},
                {"es","Agente no instalado"}
            }
        },
        {
            "No remote sessions",
            new Dictionary<string, string>() {
                {"ru","Нет удаленных сеансов"},
                {"fi","Нет удаленных сеансов"},
                {"fr","Pas de sessions à distance"},
                {"es","Sin sesiones remotas"}
            }
        },
        {
            "No active remote sessions.",
            new Dictionary<string, string>() {
                {"ru","Нет активных удаленных сеансов."},
                {"fi","Нет активных удаленных сеансов."},
                {"fr","Aucune session à distance active."},
                {"es","No hay sesiones remotas activas."}
            }
        },
        {
            "Connecting",
            new Dictionary<string, string>() {
                {"ru","Подключение"},
                {"fi","Подключение"},
                {"fr","De liaison"},
                {"es","Conectando"}
            }
        },
        {
            "Allow",
            new Dictionary<string, string>() {
                {"ru","Позволять"},
                {"fi","Позволять"},
                {"fr","Permettre"},
                {"es","Permitir"}
            }
        },
        {
            "Show &Events...",
            new Dictionary<string, string>() {
                {"ru","Показать и события ..."},
                {"fi","Показать и события ..."},
                {"fr","Afficher les &événements..."},
                {"es","Espectáculos y eventos ..."}
            }
        },
        {
            "A new version of this software is available. Update now?",
            new Dictionary<string, string>() {
                {"ru","Доступна новая версия этого программного обеспечения. Обновить сейчас?"},
                {"fi","Доступна новая версия этого программного обеспечения. Обновить сейчас?"},
                {"fr","Une nouvelle version de ce logiciel est disponible. Mettez à jour maintenant?"},
                {"es","Hay disponible una nueva versión de este software. ¿Actualizar ahora?"}
            }
        },
        {
            "Event",
            new Dictionary<string, string>() {
                {"ru","Мероприятие"},
                {"fi","Мероприятие"},
                {"fr","Événement"},
                {"es","Evento"}
            }
        },
        {
            "Deny",
            new Dictionary<string, string>() {
                {"ru","Отрицать"},
                {"fi","Отрицать"},
                {"fr","Refuser"},
                {"es","Negar"}
            }
        },
        {
            "1 remote session",
            new Dictionary<string, string>() {
                {"ru","1 удаленный сеанс"},
                {"fi","1 удаленный сеанс"},
                {"fr","1 séance à distance"},
                {"es","1 sesión remota"}
            }
        },
        {
            "{0} - {1}",
            new Dictionary<string, string>() {

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
            "Disabled",
            new Dictionary<string, string>() {
                {"de","Deaktiviertes"},
                {"hi","विकलांग"},
                {"fr","Désactivé"},
                {"zh-cht","已禁用"},
                {"zh-chs","已禁用"},
                {"fi","Poistettu käytöstä"},
                {"tr","Devre dışı"},
                {"cs","Zakázáno"},
                {"ja","無効"},
                {"es","Deshabilitado"},
                {"pt","Desativado"},
                {"nl","Uitgeschakeld"},
                {"ko","비활성화"},
                {"ru","Отключено"}
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
            "Direct Connect",
            new Dictionary<string, string>() {
                {"ru","Прямое соединение"},
                {"fi","Прямое соединение"},
                {"fr","Connection directe"},
                {"es","Conexión directa"}
            }
        },
        {
            "Not Activated (In)",
            new Dictionary<string, string>() {
                {"de","Nicht aktiviert (In)"},
                {"hi","सक्रिय नहीं (में)"},
                {"fr","Non activé (en)"},
                {"zh-cht","未啟動（輸入）"},
                {"zh-chs","未激活（输入）"},
                {"fi","Ei aktivoitu (sisään)"},
                {"tr","Etkinleştirilmedi (İçinde)"},
                {"cs","Neaktivováno (v)"},
                {"ja","アクティブ化されていない（イン）"},
                {"es","No Activada (entrada)"},
                {"pt","Não ativado (entrada)"},
                {"nl","Niet geactiveerd (In)"},
                {"ko","활성화되지 않음 (In)"},
                {"ru","Не активированно (In)"}
            }
        },
        {
            "1 remote session is active.",
            new Dictionary<string, string>() {
                {"ru","Активен 1 удаленный сеанс."},
                {"fi","Активен 1 удаленный сеанс."},
                {"fr","1 session à distance est active."},
                {"es","1 sesión remota está activa."}
            }
        },
        {
            "(None)",
            new Dictionary<string, string>() {
                {"ru","(Никто)"},
                {"fi","(Никто)"},
                {"fr","(Rien)"},
                {"es","(Ninguno)"}
            }
        },
        {
            "Desktop",
            new Dictionary<string, string>() {
                {"hi","डेस्कटॉप"},
                {"fr","Bureau"},
                {"zh-cht","桌面"},
                {"zh-chs","桌面"},
                {"fi","Työpöytä"},
                {"tr","Masaüstü Bilgisayar"},
                {"cs","Plocha"},
                {"ja","デスクトップ"},
                {"es","Escritorio"},
                {"pt","Área de Trabalho"},
                {"nl","Bureaublad"},
                {"ko","데스크탑"},
                {"ru","Рабочий стол"}
            }
        },
        {
            "Unknown",
            new Dictionary<string, string>() {
                {"de","Unbekannt"},
                {"hi","अनजान"},
                {"fr","Inconnue"},
                {"zh-cht","未知"},
                {"zh-chs","未知"},
                {"fi","Tuntematon"},
                {"tr","Bilinmeyen"},
                {"cs","Neznámé"},
                {"ja","未知の"},
                {"es","Desconocido"},
                {"pt","Desconhecido"},
                {"nl","Onbekend"},
                {"ko","알 수 없는"},
                {"ru","Неизвестно"}
            }
        },
        {
            "Activated",
            new Dictionary<string, string>() {
                {"de","Aktiviertes"},
                {"hi","सक्रिय"},
                {"fr","Activé"},
                {"zh-cht","已啟動"},
                {"zh-chs","已激活"},
                {"fi","Aktivoitu"},
                {"tr","Aktif"},
                {"cs","Zapnuto"},
                {"ja","有効化"},
                {"es","Activado"},
                {"pt","ativado"},
                {"nl","Geactiveerd"},
                {"ko","활성화 됨"},
                {"ru","Активировано"}
            }
        },
        {
            "Multiple Users",
            new Dictionary<string, string>() {
                {"ru","Несколько пользователей"},
                {"fi","Несколько пользователей"},
                {"fr","Utilisateurs multiples"},
                {"es","Múltiples usuarios"}
            }
        },
        {
            "Terminal",
            new Dictionary<string, string>() {
                {"hi","टर्मिनल"},
                {"zh-cht","終端機"},
                {"zh-chs","终端"},
                {"fi","Pääte"},
                {"tr","terminal"},
                {"cs","Terminál"},
                {"ja","ターミナル"},
                {"ko","터미널"},
                {"ru","Терминал"}
            }
        },
        {
            "&Close",
            new Dictionary<string, string>() {
                {"ru","&Закрывать"},
                {"fi","&Закрывать"},
                {"fr","&Fermer"},
                {"es","&Cerca"}
            }
        },
        {
            "Authenticating",
            new Dictionary<string, string>() {
                {"ru","Аутентификация"},
                {"fi","Аутентификация"},
                {"fr","Authentification"},
                {"es","Autenticando"}
            }
        },
        {
            "User Consent",
            new Dictionary<string, string>() {
                {"de","Benutzereinwilligung"},
                {"hi","उपयोगकर्ता सहमति"},
                {"fr","Consentement de l'utilisateur"},
                {"zh-cht","用戶同意"},
                {"zh-chs","用户同意"},
                {"fi","Käyttäjän suostumus"},
                {"tr","Kullanıcı Onayı"},
                {"cs","Souhlas uživatele"},
                {"ja","ユーザーの同意"},
                {"es","Consentimiento del Usuario"},
                {"pt","Consentimento do Usuário"},
                {"nl","Toestemming van gebruiker"},
                {"ko","사용자 연결 옵션"},
                {"ru","Согласие пользователя"}
            }
        },
        {
            "Not Activated (Pre)",
            new Dictionary<string, string>() {
                {"de","Nicht aktiviert (Pre)"},
                {"hi","सक्रिय नहीं (पूर्व)"},
                {"fr","Non activé (pré)"},
                {"zh-cht","未啟動（預）"},
                {"zh-chs","未激活（预）"},
                {"fi","Ei aktivoitu (ennakko)"},
                {"tr","Etkinleştirilmedi (Ön)"},
                {"cs","Neaktivováno (před)"},
                {"ja","アクティブ化されていない（前）"},
                {"es","No activada (Pre)"},
                {"pt","Não ativado (pré)"},
                {"nl","Niet geactiveerd (Pre)"},
                {"ko","활성화되지 않음 (Pre)"},
                {"ru","Не активированно (Pre)"}
            }
        },
        {
            "S&top Agent",
            new Dictionary<string, string>() {
                {"ru","S & главный агент"},
                {"fi","S & главный агент"},
                {"fr","Agent d'arrêt"},
                {"es","Agente s & top"}
            }
        },
        {
            "UDP relay",
            new Dictionary<string, string>() {
                {"ru","UDP реле"},
                {"fi","UDP реле"},
                {"fr","Relais UDP"},
                {"es","Relé UDP"}
            }
        },
        {
            "Agent is stopped",
            new Dictionary<string, string>() {
                {"ru","Агент остановлен"},
                {"fi","Агент остановлен"},
                {"fr","L'agent est arrêté"},
                {"es","El agente está detenido"}
            }
        },
        {
            "Intel® Management Engine",
            new Dictionary<string, string>() {
                {"fr","Moteur de gestion Intel®"},
                {"es","Motor de administración Intel®"}
            }
        },
        {
            "Agent Snapshot",
            new Dictionary<string, string>() {
                {"ru","Снимок агента"},
                {"fi","Снимок агента"},
                {"fr","Instantané de l'agent"},
                {"es","Instantánea del agente"}
            }
        },
        {
            "Request Help...",
            new Dictionary<string, string>() {
                {"ru","Запросить помощь ..."},
                {"fi","Запросить помощь ..."},
                {"fr","Demander de l'aide..."},
                {"es","Solicitar ayuda ..."}
            }
        },
        {
            "O&pen Site...",
            new Dictionary<string, string>() {
                {"ru","Открытие сайта ..."},
                {"fi","Открытие сайта ..."},
                {"fr","&Ouvrir le site..."},
                {"es","Sitio de O & pen ..."}
            }
        },
        {
            "Send",
            new Dictionary<string, string>() {
                {"de","Senden"},
                {"hi","संदेश"},
                {"fr","Envoyer"},
                {"zh-cht","發送"},
                {"zh-chs","发送"},
                {"fi","Lähetä"},
                {"tr","Gönder"},
                {"cs","Odeslat"},
                {"ja","送る"},
                {"es","Enviar"},
                {"pt","Enviar"},
                {"nl","Verzenden"},
                {"ko","전송"},
                {"ru","Отправить"}
            }
        },
        {
            "Clear",
            new Dictionary<string, string>() {
                {"de","Leeren"},
                {"hi","स्पष्ट"},
                {"fr","Nettoyer"},
                {"zh-cht","清除"},
                {"zh-chs","清除"},
                {"fi","Tyhjennä"},
                {"tr","Açık"},
                {"cs","Vymazat"},
                {"ja","クリア"},
                {"es","Borrar"},
                {"pt","Limpo"},
                {"nl","Wissen"},
                {"ko","지우기"},
                {"ru","Очистить"}
            }
        },
        {
            "Cancel Help Request",
            new Dictionary<string, string>() {
                {"ru","Отменить запрос помощи"},
                {"fi","Отменить запрос помощи"},
                {"fr","Annuler la demande d'aide"},
                {"es","Cancelar solicitud de ayuda"}
            }
        },
        {
            "TCP relay",
            new Dictionary<string, string>() {
                {"ru","Реле TCP"},
                {"fi","Реле TCP"},
                {"fr","Relais TCP"},
                {"es","Relé TCP"}
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
            "Agent is running",
            new Dictionary<string, string>() {
                {"ru","Агент работает"},
                {"fi","Агент работает"},
                {"fr","L'agent est en cours d'exécution"},
                {"es","El agente se está ejecutando"}
            }
        },
        {
            "Agent Console",
            new Dictionary<string, string>() {
                {"de","Agent-Konsole"},
                {"hi","एजेंट कंसोल"},
                {"fr","Console d'agent"},
                {"zh-cht","代理控制台"},
                {"zh-chs","代理控制台"},
                {"fi","Agentin konsoli"},
                {"tr","Aracı Konsolu"},
                {"cs","Konzole agenta"},
                {"ja","エージェントコンソール"},
                {"es","Consola de Agente"},
                {"pt","Console do agente"},
                {"nl","Agent console"},
                {"ko","에이전트 콘솔"},
                {"ru","Консоль агента"}
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
