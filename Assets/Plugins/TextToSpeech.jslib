mergeInto(LibraryManager.library, {
    
    CheckTTSBrowserSupported: function(){
        if ('speechSynthesis' in window) return true;
        return false;
    },

    SetupTextToSpeech: function(soundVolume, speakRate, speakPitch, gameObjectName, onEndCallbackName){
        utterance = new SpeechSynthesisUtterance();
        utterance.voice = speechSynthesis.getVoices().filter(voice => voice.lang.startsWith("en"))[1];
        utterance.volume = soundVolume;
        utterance.rate = speakRate;
        utterance.pitch = speakPitch;
        utterance.lang = 'en-US';
        
        gameObject = UTF8ToString(gameObjectName);
        onEndCallback = UTF8ToString(onEndCallbackName);

        utterance.onend = function(event){
            if(event.charIndex == this.text.length){
                SendMessage(gameObject, onEndCallback);
            }
        }
    },

    StopSpeaking: function() {
        speechSynthesis.cancel()
    },

    Speak: function(textToSpeak) {
        utterance.text = UTF8ToString(textToSpeak);
        speechSynthesis.speak(utterance);
    },

    Pause: function() {
        speechSynthesis.pause();
    },

    Resume: function() {
        speechSynthesis.resume();
    },

    GetVoices: function() {
        var voices = speechSynthesis.getVoices().filter(voice => voice.lang.startsWith("en"));
        var voiceNames = "";
        for (var i = 0; i < voices.length; i++) {
            voiceNames += voices[i].name;
            if (i < voices.length - 1) {
                voiceNames += "#";
            }
        }
        var bufferSize = lengthBytesUTF8(voiceNames) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(voiceNames, buffer, bufferSize);
        return buffer;
    },

    SetVoice: function(voiceIndex){
        utterance.voice = speechSynthesis.getVoices().filter(voice => voice.lang.startsWith("en"))[voiceIndex];
    },

    GetVolume: function(){
        return utterance.volume;
    },

    SetVolume: function(volume){
        utterance.volume = volume;
    },

    GetSpeakingRate: function(){
        return utterance.rate;
    },
    
    SetSpeakingRate: function(speakingRate){
        utterance.rate = speakingRate;
    }
});
