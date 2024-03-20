mergeInto(LibraryManager.library, {
    gameObject: "",
    liveTranscribeCallback: "",
    finalResultCallback: "",
    recognition: "",
    isListening: false,

    CheckBrowserSupported: function(){
        if ('webkitSpeechRecognition' in window) return true;
        return false;
    },

    SetupSpeechRecognition: function(gameObjectName, liveTranscribeCallbackName, finalResultCallbackName) {
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
                this.isListening = true;
                console.log("true" + this.isListening);
            };

            recognition.onaudiostart = function () {
                console.log(this);
                this.isListening = true;
                console.log("true" + this.isListening);
            };

            recognition.onend = function () {
                console.log(this);
                this.isListening  = false;
                console.log("false" + this.isListening);
            };
            
            recognition.onerror = function (event) {
                console.log(this);
                this.isListening  = false;
                console.log("false" + this.isListening);
            };

    },

    StartSpeechRecognition: function() {
            console.log(this)
            if(!this.isListening){
                this.recognition.start();
            }
    },

    IsListening: function() {
        console.log("isListening: " + this.isListening)
        if(this.isListening) return true;
        return false;
    }

});
