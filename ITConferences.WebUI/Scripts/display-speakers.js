$m = jQuery.noConflict();

//autocomplete location

$m(document).ready(function () {

    var page = 0;

    //pass parameters to filter conferences by click enter
    $('.submit').keypress(function (e) {
        if (e.which == 13) {
            updateSpeakers();
        }
    });

    //pass parameters to filter conferences
    $m("#submitButton").click(function () {
        updateSpeakers();
    });

    function updateSpeakers() {
        $m("#loading").show();
        $("#speakers").hide();
        var filterTrue = true;

        $m.ajax({
            url: '/Speakers/GetSpeakers/',
            type: "POST",
            dataType: "HTML",
            data: { nameFilter: $m("#name").val(), filter: filterTrue },
            success: function (data) {
                $m("#speakers").empty();
                $m("#speakers").html(data);

            },
            complete: function () {
                $m("#loading").hide();
                $m("#speakers").show();
                page = 0;
            }
        });
    }


    //paging site by scrolling
    var lastScrollTop = 0;
   
    var inCallback = false;

    function loadSpeakers() {
        if (page > -1 && !inCallback) {
            inCallback = true;
            page++;
            $m.ajax({
                url: '/Speakers/GetSpeakers/',
                type: "POST",
                dataType: "HTML",
                data: { page: page },
                success: function (data) {
                    if (data != '') {
                        $m("#speakers").append(data);
                    } else {
                        page = -1;
                    }
                    
                },
                complete: function () {
                    inCallback = false;
                }
                
            });
        }

    }


    $m(window).scroll(function () {
        var st = $(this).scrollTop();
        if (($m(window).scrollTop() <= $m(document).height() - $m(window).height()) && (st > lastScrollTop)) {
            loadSpeakers();
        }
        lastScrollTop = st;
    });




    

});