ng.ready(function () {
    $(function () {
        $("#datepicker").datepicker({
            dateFormat: 'yymmdd',
            altField: '#alternate',
            altFormat: 'DD، d MM yy'
        });
    });
});