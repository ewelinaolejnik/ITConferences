$m = jQuery.noConflict();

//autocomplete location
$m(document).ready(function () {
    $m("#location").autocomplete({

        source: function (request, response) {

            $m.ajax({

                url: '/Conferences/GetLocations/',
                type: "POST",
                dataType: "JSON",
                data: { locationFilter: $m("#location").val() },
                success: function (data) {
                    response(data);
                },
                error: function (res) {
                    alert("Error" + res.responseText);
                }
            });
        }
    });

    //create multiselect tags
    $m("#tags").multiselect({
        noneSelectedText: "Select tags",
        selectedList: 4
    }).multiselectfilter();

    var page = 0;

    //pass parameters to filter conferences
    $m("#submitButton").click(function () {

        $m("#loading").show();
        $("#conferences").hide();

        $m.ajax({
            url: '/Conferences/GetConferences/',
            type: "POST",
            dataType: "HTML",
            data: { selectedTagsIds: $m("#tags").val(), locationFilter: $m("#location").val(), nameFilter: $m("#name").val(), dateFilter: $m("#dateFilter").val() },
            success: function (data) {
                $m("#conferences").html(data);

            },
            complete: function () {
                $m("#loading").hide();
                $m("#conferences").show();
                page = 0;
            }
        });


    });


    //paging site by scrolling
    var lastScrollTop = 0;
   
    var inCallback = false;

    function loadConferences() {
        if (page > -1 && !inCallback) {
            inCallback = true;
            page++;
            $m.ajax({
                url: '/Conferences/GetConferences/',
                type: "POST",
                dataType: "HTML",
                data: { page: page },
                success: function (data) {
                    if (data != '') {
                        $m("#conferences").append(data);
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
        if (($m(window).scrollTop() == $m(document).height() - $m(window).height()) && (st > lastScrollTop)) {
            loadConferences();
        }
        lastScrollTop = st;
    });
});