mergeInto(LibraryManager.library, {
    gameObject: "",
    callback: "",

    StartSpeechRecognition: function(gameObjectName, callbackName) {
        if ('webkitSpeechRecognition' in window) {
            var recognition = new webkitSpeechRecognition();
            recognition.continuous = true;
            recognition.interimResults = true;
            gameObject = UTF8ToString(gameObjectName);
            callback = UTF8ToString(callbackName);

            recognition.onresult = function(event) {
                SendMessage(gameObject, callback, event.results[event.results.length - 1][0].transcript);
            };

            recognition.start();
        } else {
            console.error('Speech recognition is not supported in this browser.');
        }
    }
});
