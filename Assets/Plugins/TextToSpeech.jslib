mergeInto(LibraryManager.library, {
    
    CheckBrowserSupported: function(){
        if ('webkitSpeechRecognition' in window) return true;
        return false;
    },

    SetupSpeechRecognition: function(gameObjectName, liveTranscribeCallbackName, finalResultCallbackName) {
            isListening = false;
            recognition = new webkitSpeechRecognition();
            recognition.continuous = false;
            recognition.interimResults = true;
            gameObject = UTF8ToString(gameObjectName);
            liveTranscribeCallback = UTF8ToString(liveTranscribeCallbackName);
            finalResultCallback = UTF8ToString(finalResultCallbackName);

            recognition.onresult = function(event) {
                if(!event.results[0].isFinal){
                    SendMessage(gameObject, liveTranscribeCallback, event.results[0][0].transcript);
                } else{
                    SendMessage(gameObject, finalResultCallback, event.results[0][0].transcript);
                }
            };

            recognition.onstart = function () {
                console.log(this);
                isListening = true;
                console.log("true" + isListening);
            };

            recognition.onaudiostart = function () {
                console.log(this);
                isListening = true;
                console.log("true" + isListening);
            };

            recognition.onend = function () {
                console.log(this);
                isListening  = false;
                console.log("false" + isListening);
            };
            
            recognition.onerror = function (event) {
                console.log(this);
                isListening  = false;
                console.log("false" + sListening);
            };

    },

    StartSpeechRecognition: function() {
            console.log(this);
            if(!isListening){
                recognition.start();
            }
    },

    IsListening: function() {
        if(isListening) return true;
        return false;
    },
});
