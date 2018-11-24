<%@ Page Language="C#" %>

<%@ Register Src="~/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>


<!DOCTYPE html>

<script runat="server">

    private void calc_OnServerClick(object sender, EventArgs e)
    {
        DateTime beginDateTime;
        var endDateTime = DateTime.Now;
        if (!DateTime.TryParse(age.Value, out beginDateTime))
        {
            result.InnerText = "Error";
            return;
        }
        if (beginDateTime > endDateTime)
            throw new Exception("开始时间应小于或等与结束时间！");
        /*计算出生日期到当前日期总月数*/
        int Months = endDateTime.Month - beginDateTime.Month + 12 * (endDateTime.Year - beginDateTime.Year);
        /*出生日期加总月数后，如果大于当前日期则减一个月*/
        int totalMonth = (beginDateTime.AddMonths(Months) > endDateTime) ? Months - 1 : Months;
        /*计算整年*/
        int fullYear = totalMonth / 12;
        /*计算整月*/
        int fullMonth = totalMonth % 12;
        /*计算天数*/
        DateTime changeDate = beginDateTime.AddMonths(totalMonth);
        double days = (endDateTime - changeDate).TotalDays;
        result.InnerText = string.Format("year={0}   month={1}   day={2:N1}", fullYear, fullMonth, days);
    }

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Age</title>
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/clipboard.min.js"></script>
    <link rel="stylesheet" href="Content/bootstrap.min.css" />
    <link href="Content/vs.css" rel="stylesheet" />
</head>
<body>
    <uc1:Menu runat="server" ID="Menu" />
<h1>年龄计算器</h1>
    <form id="form1" runat="server">
    <div class="text-center" style="margin: 0 25%">
        <br/>
        <input id="age" class="form-control" runat="server" placeholder="YYYY-MM-DD"/><br/>
        <a href="#" id="calc" runat="server" class="btn btn-primary" OnServerClick="calc_OnServerClick">计算年龄</a><br/><br/>
        <label id="result" runat="server"></label>
    </div>
    </form>
</body>
</html>
