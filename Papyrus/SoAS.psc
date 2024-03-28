Scriptname SoAS Native Hidden Const

; plugin's version
string function GetVersionString() global native 
; plugin's version
int function GetVersionInt() global native 

; starts data reloading process. should never by called by a script. call it from console
; returns 
;    true if data reload started successfuly
;    false if data reload started already or error occured
bool function StartDataReload() global native

; Subscribe for notification about data reload finish 
; handler - script with the function of the following signature
;   function OnScenesReloadFinished(bool succeed)
;    ...
;   endfunction 
; 
; returns true on success
; returns false if handler is None or it is registered already
bool function RegisterForDataReloadFinish(ScriptObject handler) global native
bool function IsDataReloadFinishRegistered(ScriptObject handler) native global
bool function UnregsiterForDataReloadFinish(ScriptObject handler) global native
struct ScriptDescriptor
    int FormId
    int HighWord
    string Name
endstruct
ScriptDescriptor function GetSubscribedDescriptor(ScriptObject handler) native global
bool function UnregisterForDataReloadFinishByDescriptor(ScriptDescriptor handler) native global


;===================
; Scene participants
;-------------------

struct Participant
    string Skeleton
    int Sex ; 0 - Any, 1 - Male, 2 - Female
    bool IsAggressor
    bool IsVictim
    ; TODO: other attributes
endstruct

; returns
;   array of participants defined for the scene
;   None - scene not found or data is been reloading
Participant[] function GetParticipants(string sceneId) global native



;===========================
; Scene participant contacts
;---------------------------

; pain types
; 0 - None
; 1 - Cold
; 2 - Hot
; 3 - Electro
; 4 - Acid
; 5 - Pierce
; 6 - Slash
; 7 - Crush

struct ParticipantsContact
    ; contact 
    ; from participant:

    int From_ParticipantIndex ; index of the participant returned by GetParticipants
    Actor From_Participant
    string From_Area
    int From_Stimulation
    int From_Hold
    int From_Pain
    int From_Comfort
    int From_Tickle
    int From_PainType

    ; to participant:
    int To_ParticipantIndex ; index of the participant returned by GetParticipants
    Actor To_Participant
    string To_Area
    int To_Stimulation
    int To_Hold
    int To_Pain
    int To_Comfort
    int To_Tickle
    int To_PainType
endstruct

; returns
;    array of contacts between actors defined for the existing scene
;    None - scene not found or data is been reloading
var[] function GetParticipantsContacts(string sceneId, string structName = "SoAS#ParticipantsContact", Actor[] participants = None) global native

struct EnvironmentContact
    int Direction ; 0 - from environment to actor, 1 - from actor to environment
    int ParticipantIndex ; index of the participant returned by GetParticipants
    Actor Participant
    string Area
    int Stimulation
    int Hold
    int Pain
    int Comfort
    int Tickle
    int PainType
endstruct

; returns
;    array of contacts between actors and environment defined for the existing scene
;    None - scene not found or data is been reloading
var[] function GetEnvironmentContacts(string sceneId, string structName = "SoAS#EnvironmentContact", Actor[] participants = None) global native

; ====================
; Scene attributes
; --------------------

; each field contains corresponding string values separated by comma
struct SceneStringAttributes
    string Authors
    string Narrative
    string Feeling
    string Service
    string Attribute
    string Other
endstruct 
; returns
;   summary of the categorized tags 
;   None - scene not found or data is been reloading
SceneStringAttributes function GetStringAttributes(string sceneId) global native

; returns 
;   tags associated with the scene
;   None - scene not found or data is been reloading
string[] function GetTags(string sceneId) global native

; returns 
;   array of furniture tags associated with the existing scene. will be empty if no furniture associated with the scene
;   None - scene not found or data is been reloading
string[] function GetFurnitureTags(string sceneId) global native


; returns 
;   struct object with fields populated by exsting data
;     fields mapped by name, case insensitivy
;     in case of type mismatch, field has default value for its type.
;     in case when stored value is an object or an array, value converted to the JSON string
;   None - scene not found or structName does not represent existing structure
; Note: naming convention: to retrieve custom attributes field name need to be declared with Custom_ prefix, for example Custom_MyField
var function GetSceneAttributes(string sceneId, string structName) global native