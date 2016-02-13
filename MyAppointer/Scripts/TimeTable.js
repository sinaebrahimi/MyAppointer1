$(document).ready(function () {
    var weekdays = ["شنبه", "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنحشنبه", "جمعه"];
    
    $("#datepicker").datepicker({
        dateFormat: 'yy-mm-dd',
        altField: '#alternate',
        altFormat: 'DD'
    }).change(function () {
        $(".day-name").html($("#alternate").val());
        $("#BookDate").val($(this).val().replace(/-/g, ''));
        var workingday_flag = 0;
        for (i = 0; i < 7; i++) {
            if ($(".day_wrapper_" + i).val() != "undefined") {
                if (weekdays[parseInt($(".day_wrapper_" + i).val())] == $(".alternate").val()) {
                    var workingday_flag = 1;
                    break;
                }
            } else {
                break;
            }
        }

        if (workingday_flag == 1) {

            $(".times-row").attr("data-toggle", "modal");
            $(".times-row").removeClass('disabled');
            $.ajax({
                type: "GET",
                url: "../api/Appointment/" + $("#BookDate").val() + "/" + $("#JobOwnerId").val(),
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
                        $(".progress.appointments").append("<div class='progress-bar progress-bar-warning' style='width: " + percent + "%;right:" + right + "%'></div>");
                    });
                }
            });
        } else {
            $(".times-row").attr("data-toggle", "");
            $(".times-row").addClass('disabled');
            $(".progress.appointments").html("");
        }

    }).val("1394-11-24").change();
    $('.clockpicker').clockpicker();
    $('#StartTimeClock').blur(function () {

        var x = $('#StartTimeClock').val();

        var res = x.split(":");

        var parseClock = parseInt(res[0], 10);

        var parseMinute = parseInt(res[1],10);
        parsed = (parseClock * 60) + (parseMinute);
        
        document.getElementById("StartTime").value = parsed;


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
        flag=0;
        for (i = 0; i < 2; i++) {
            if ($(".time_wrapper_" + i).val() != "undefined") {
                var strs = $(".time_wrapper_" + i).val();
                if (parseInt(strs) != "undefined") {
                    var str = strs.split('-');
                    if (parseInt($("#StartTime").val()) > parseInt(str[0]) && parseInt($("#EndTime").val()) < parseInt(str[1])) {
                        var flag = 1;
                        break;
                    }
                }
            }
        }
        if (flag.toString()=="0") {
            alert("error in Time Range");
            alert(parseInt($("#StartTime").val()) + 30);
        } else {
            $("#EndTime").val(parseInt($("#StartTime").val())+30);
            var data = JSON.stringify($('form.reserve-form').serializeObject());
            $.ajax({
                type: "POST",
                url: "../api/Appointment",
                data: data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg == "overlap") {
                        alert('selected time have been reserved before');
                    } else {
                        alert("new Appointment create successfully")
                        $("#myModal").modal('toggle');
                    }
                }
            });
        }
    });
    
});