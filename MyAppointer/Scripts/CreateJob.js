$(function(){
    var i = 1;
    var j = 1;
    $("#Add_day").click(function(){
        $(".editor-field.days-wrapper").append('<br/>Day['+i+']<br/><input class="text-box single-line" data-val="true" data-val-number="The field Day must be a number." data-val-required="The Day field is required." id="WeeklyWorkingDays_' + i + '__Day" name="WeeklyWorkingDays[' + i + '].Day" type="number" value=""><input data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id="WeeklyWorkingDays_'+i+'__Id" name="WeeklyWorkingDays['+i+'].Id" type="hidden" value="">');
        i++;
    });
    
    $("#Add_Time").click(function () {
        $(".editor-field.times-wrapper").append('<br/>Time['+j+']<br/><input data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id="WeeklyWorkingTimes_' + j + '__Id" name="WeeklyWorkingTimes[' + j + '].Id" type="hidden" value=""><input class="text-box single-line" data-val="true" data-val-number="The field StartTime must be a number." data-val-required="The StartTime field is required." id="WeeklyWorkingTimes_' + j + '__StartTime" name="WeeklyWorkingTimes[' + j + '].StartTime" type="number" value=""><input class="text-box single-line" data-val="true" data-val-number="The field EndTime must be a number." data-val-required="The EndTime field is required." id="WeeklyWorkingTimes_' + j + '__EndTime" name="WeeklyWorkingTimes[' + j + '].EndTime" type="number" value="">');
        j++;
    });
    $("#Jobs_JobTypeId").val($("#JobTypeId").val());
    $("#JobTypeId").change(function () {
        $("#Jobs_JobTypeId").val($(this).val());
    });


});