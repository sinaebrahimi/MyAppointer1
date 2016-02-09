$(document).ready(function () {
    var days = $('#daysNumber').val();
    for (i = 0; i < days.length; i++) {
        $('#TimeTable tbody tr:nth-child(' + days.charAt(i) + ')').removeClass('disabled');
    }

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