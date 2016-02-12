$(document).ready(function () {
    $("#datepicker").datepicker({
        dateFormat: 'yymmdd',
        altField: '#alternate',
        altFormat: 'DD'
    }).change(function () {
        $(".day-name").html($("#alternate").val());
        $("#BookDate").val($(this).val());

        $.ajax({
            type: "GET",
            url: "../api/Appointment/"+$(this).val()+"/"+$("#JobOwnerId").val(),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var obj = jQuery.parseJSON(msg);

                var startTime, endtime;
                var percent, right;
                var LastStartTime = 420;
                $(".progress.appointments").html("");

                $.each(obj, function (i, object) {
                    $.each(object, function (key, val) {
                        if (key == "startTime") {
                            startTime = val;
                        } else if (key == "endTime") {
                            endtime = val;
                        }
                    });

                    percent = 100 * ((endtime - startTime) / 840);
                    right = 100 * ((startTime - 420) / 840);

                    $(".progress.appointments").append("<div class='progress-bar progress-bar-warning' style='width: " + percent + "%;right:" + right + "%'></div>"
)

                    
                });


               
               
            }
        });


    });


    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    $('#submit-appointment').click(function () {
        var data = JSON.stringify($('form.reserve-form').serializeObject());
        $.ajax({
            type: "POST",
            url: "../api/Appointment",
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if(msg == "overlap") {
                	alert('selected time have been reserved before');
                }else{
                	alert("new Appointment create successfully")
                }
            }
        });
    });


    $('.clockpicker').clockpicker();
    $('#StartTimeClock').blur(function () {

        var x = $('#StartTimeClock').val();

        var res = x.split(":");

        var parseClock = parseInt(res[0], 10);

        var parseMinute = parseInt(res[1],10);
        parsed = (parseClock * 60) + (parseMinute);
        
        //$('#StartTime').val() = parsed;
        document.getElementById("StartTime").value = parsed;




    });
});