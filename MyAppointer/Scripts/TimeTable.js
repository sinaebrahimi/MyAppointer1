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
            url: "../api/Appointment/"+$(this).val()+"/4",
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

    var times = $('#dayTimes').val();
    TimeParts = times.split(',');
    var PreviousStartTime = 420;
    for (i = 0; i < TimeParts.length-1; i++) {
        time = TimeParts[i].split('-');
        StartTime = parseInt(time[0].substring(0, time[0].length - 2)) * 60 + parseInt(time[0].substring(2));
        EndTime = parseInt(time[1].substring(0, time[1].length - 2)) * 60 + parseInt(time[1].substring(2));
        percent = 100*((EndTime - StartTime) / 840);
        right = 100 * ((StartTime - PreviousStartTime) / 840);
        PreviousStartTime = EndTime;
        //alert(StartTime);
        $('#TimeTable tbody tr:not( .disabled) .times').append('<div class="progress-bar progress-bar-success" style="width: ' + percent + '%;right:'+right+'%"></div>');

    }

    
});