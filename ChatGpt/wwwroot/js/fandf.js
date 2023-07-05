let convo
var enterkey = true;
const delay = ms => new Promise(res => setTimeout(res, ms));
const walker = "[Paul Walker]:";
const dom = "[Dominic Toretto]:";

$(document).ready(function () {
    // Listen for keyup event on the input field
    $("#chatPrompt").keyup(function (event) {
        // Check if the key code is 13 (Enter key)
        if (event.keyCode === 13 && enterkey) {
            // Call the function
            callChatGtp();
        }
    });
});

function callChatGtp() {
    //Get the
    const prompt = $("#chatPrompt").val();

    //Make the Ajax request
    $.ajax({
        type: "POST",
        url: "/FandF/CallChatGtp",
        data: {
            msg: prompt
        },
        //Returns a ajaxbase object, with error code 0 if fine 1 if saving error
        //-- Display the correct message --
        success: function (response) {
            var convo = response;
            var spiltConvo = convo[0].split("\n");

            let i = 0;
            var voices = window.speechSynthesis.getVoices();

            function processNextItem() {

                var divclass;
                //get him to read it out
                var msg = new SpeechSynthesisUtterance();
                msg.text = response.response;

                if (i < spiltConvo.length) {
                    if (spiltConvo[i].includes(walker)) {
                        divclass = "<div class='paul'>";
                        msg.voice = voices[6];
                        msg.pitch = 2;
                        msg.rate = 3;
                        msg.text = spiltConvo[i].replace("[Paul Walker]:","");
                        msg.onend = processNextItem;
                        window.speechSynthesis.speak(msg);
                    }
                    else if (spiltConvo[i].includes(dom)) {
                        divclass = "<div class='dom'>";
                        msg.pitch = 0;
                        msg.rate = 1;
                        msg.voice = voices[3];
                        msg.text = spiltConvo[i].replace("[Dominic Toretto]:", "");
                        msg.onend = processNextItem;
                        window.speechSynthesis.speak(msg);
                    }
                    else {
                        divclass = "<div>";
                        msg.pitch = 1;
                        msg.voice = voices[1];
                        msg.text = spiltConvo[i];
                        msg.onend = processNextItem;
                        window.speechSynthesis.speak(msg);
                    }
                    var script = $("#script").html();
                    script = script + divclass + spiltConvo[i] + "</div>";
                    $("#script").html(script);
                    $("#script").scrollTop($("#script")[0].scrollHeight);
                    i++;

                    //setTimeout(processNextItem, 1000); // Wait for 2 seconds before processing the next item
                }
            }

            processNextItem();
        },
        //If we get not response throw a wobbly
        error: function (response) {
            alert("Error");  //
        }
    })
}