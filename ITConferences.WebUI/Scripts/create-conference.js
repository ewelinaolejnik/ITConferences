//for fill date
$('.datepicker').datepicker();

//for fill cities
$(document).ready(function () {
    $("#TargetCountryId").change(function () {
        $.ajax({
            url: 'GetSelectedCities',
            type: "POST",
            dataType: "JSON",
            data: { countryID: $(this).val() },
            success: function (data) {
                $("#TargetCityId").empty(); // clear before appending new list 
                var items = '<option>Select city</option>';
                $.each(data, function (i, city) {
                    items += "<option value='" + city.Value + "'>" + city.Text + "</option>";
                });
                $('#TargetCityId').html(items);
            },
            error: function (ex) {
                alert('Failed to retreive cities. ' + ex);
            }
        });
    });
});

////for create cascading drop down list with cities
//function fillCity() {
//    var country = $('#Country');
//    $.ajax({
//        url: 'Conferences/GetCities',
//        type: "POST",
//        dataType: "JSON",
//        data: { country: country },
//        success: function (cities) {
//            $("#City").html(""); // clear before appending new list 
//            $.each(cities, function (i, city) {
//                $("#City").append(
//                    $('<option></option>').val(city.CityID).html(city.Name));
//            });
//        },
//        error: function (ex) {
//            alert('Failed to retreive cities. ' + ex);
//        }
//    });
//}