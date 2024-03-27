mergeInto(LibraryManager.library, {
    
    CheckTTSBrowserSupported: function(){
        if ('speechSynthesis' in window) return true;
        return false;
    },

    SetupTextToSpeech: function(speakVoice, soundVolume, speakRate, speakPitch, gameObjectName, onEndCallbackName){
        utterance = new SpeechSynthesisUtterance();
        utterance.voice = speechSynthesis.getVoices()[speakVoice];
        utterance.volume = soundVolume;
        utterance.rate = speakRate;
        utterance.pitch = speakPitch;

        
        gameObject = UTF8ToString(gameObjectName);
        onEndCallback = UTF8ToString(onEndCallbackName);

        utterance.onend = function(){
            SendMessage(gameObject, onEndCallbackName);
        }
    },

    StopSpeaking: function() {
        speechSynthesis.cancel()
    },

    Speak: function(textToSpeak) {
        utterance.text = textToSpeak;
        speechSynthesis.speak(utterance);
    }
});
