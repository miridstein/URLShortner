$(() => {
    $("#submit").on('click', function () {
        const url = $("#long-url").val();
        console.log('hello')
        console.log(url)
       
        $.post('/home/shortenUrl', { url }, function (u) {
            console.log(u.hashedUrl);
            $("#short-url").html(`<a href='${u.hashedUrl}'>${u.hashedUrl}</a>`);
           // $("#shortened-url").slideDown();
        });
    });
})