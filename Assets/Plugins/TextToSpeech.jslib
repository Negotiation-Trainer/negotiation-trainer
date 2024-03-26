mergeInto(LibraryManager.library, {
    
    CheckTTSBrowserSupported: function(){
        if ('speechSynthesis' in window) return true;
        return false;
    },

    Speak: function() {
        var utterance = new SpeechSynthesisUtterance('Hello, world!');
        utterance.voice = speechSynthesis.getVoices()[0];
        speechSynthesis.speak(utterance);
    }
});
