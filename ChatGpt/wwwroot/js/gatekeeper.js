// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let convo
var enterkey = true;

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

$("#send").on("click", function (e) {
    //e.preventDefault();
    callChatGtp();
})

function callChatGtp() {
    //Get the
    const prompt = $("#chatPrompt").val();
    convo = $("#conversation").html();
    convo += `<br>[You]: ${prompt}`;
    $("#conversation").html(convo);
    $("#chatPrompt").val("");
    $("#conversation").scrollTop($("#conversation")[0].scrollHeight);

    //Make the Ajax request
    $.ajax({
        type: "POST",
        url: "/GateKeeper/CallChatGtp",
        data: {
            msg: prompt
        },
        //Returns a ajaxbase object, with error code 0 if fine 1 if saving error
        //-- Display the correct message --
        success: function (response) {
            convo += `<br>[Guard]: ${response.response}`;
            $("#conversation").html(convo);

            //get him to read it out
            var msg = new SpeechSynthesisUtterance();
            msg.text = response.response;
            msg.rate = 8;
            msg.rate = 1;
            window.speechSynthesis.speak(msg);

            if (response.isThrough) {
                convo += "<br>YOU GOT IN, CONGRATS<br>Thank you for playing";
                $("#conversation").html(convo);
                $("#send").prop('disabled', true);
                enterkey = false;
            }
            $("#conversation").scrollTop($("#conversation")[0].scrollHeight);
        },
        //If we get not response throw a wobbly
        error: function (response) {
            alert("Error");  //
        }
    })
}