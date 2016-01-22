$m = jQuery.noConflict();


$m(document).ready(function () {

    $m("#TargetCountryId").change(function () {
        $m.ajax({
            url: 'GetSelectedCities',
            type: "POST",
            dataType: "JSON",
            data: { countryID: $m(this).val() },
            success: function (data) {
                $m("#TargetCityId").empty(); // clear before appending new list 
                var items = '<option>Select city</option>';
                $m.each(data, function (i, city) {
                    items += "<option value='" + city.Value + "'>" + city.Text + "</option>";
                });
                $m('#TargetCityId').html(items);
            },
            error: function (ex) {
                alert('Failed to retreive cities. ' + ex);
            }
        });
    });


    //for create cascading drop down list with cities
    var country = $m('#Country');
    $m.ajax({
        url: 'Conferences/GetCities',
        type: "POST",
        dataType: "JSON",
        data: { country: country },
        success: function (cities) {
            $m("#City").html(""); // clear before appending new list 
            $m.each(cities, function (i, city) {
                $m("#City").append(
                    $m('<option></option>').val(city.CityID).html(city.Name));
            });
        },
        error: function (ex) {
            alert('Failed to retreive cities. ' + ex);
        }

    });

});