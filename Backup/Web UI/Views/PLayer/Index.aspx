<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ViewMasterPage1.Master" Inherits="System.Web.Mvc.ViewPage<Player>" %>
<%@ Import Namespace="Web_UI.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%

 %>

<h2><img src="http://<%= Model.WowPlayer.Region.ToLower() %>.battle.net/static-render/<%= Model.WowPlayer.Region.ToLower() %>/<%= Model.WowPlayer.Thumbnail%>" alt="" /><%= Model.WowPlayer.Name %> </h2>
&lt;<span class="clusterName"><%= ClusterHelper.GetClusterName(Model.WowPlayer.ClusterNumber) %></span> of <span class="guild"><%= Model.WowPlayer.Guild %></span>&gt;<br />
Level <span class="level"><%= Model.WowPlayer.Level %></span> <span class="race"><%= Model.WowPlayer.Race %></span> <span class="class"><%= Model.WowPlayer.Class %></span>

<div class="playerTop">
<div class="recentAchievements">
<h3>Recent Achivements</h3>
<%
    
    var achievements = Model.RecentAchievements;

    foreach (var cAchievement in achievements)
    {
        
        Html.RenderPartial("AchievementBlock", cAchievement);
        %>
        <br />
        <%
    }
  
  
   %>
</div>
<div class="playerStatus">
<div class="innerPlayerStatus">
<h3>Player Status</h3>

<div id="progress_bar" class="ui-progress-bar ui-container">
      <div class="ui-progress" style="width: <%= Math.Round(((Model.WowPlayer.Achievements.Count) / (double)(Model.WowPlayer.Achievements.Count +  + Model.RecommendedAchievements.Count)) * 100, 0 ) %>%;">
        <span class="ui-label" >Completed <b class="value"><%= Model.WowPlayer.Achievements.Count %></b> of <%= Model.WowPlayer.Achievements.Count + Model.RecommendedAchievements.Count%></span>
      </div>
    </div>

    <%
        if (!string.IsNullOrEmpty(Model.WowPlayer.Guild))
        {
            %>
            #<%= Model.GuildPostion +1 %> of <%= Html.ActionLink(Model.WowPlayer.Guild, "Guild", "Ranking", new { guildName = Model.WowPlayer.Guild, server = Model.WowPlayer.Server, region = Model.WowPlayer.Region}, new { })%> <br /><%
        }
         %>

#<%= Model.ServerPosition + 1%> of <%= Html.ActionLink(Model.WowPlayer.Server, "ServerRanking", "Ranking", new { server = Model.WowPlayer.Server, region = Model.WowPlayer.Region }, new { })%> <br />
#<%= Model.WorldWidePositon  + 1%> Overall <br />
<%= Html.ActionLink("Reparse", "Parse", new {name=Model.WowPlayer.Name, server=Model.WowPlayer.Server, region=Model.WowPlayer.Region}) %>  <br />Last Parsed: <%= Model.WowPlayer.LastParseDate.Value.ToShortDateString() %><br />
</div>
</div>
</div>
<div class="clear"></div>


<h3>Recommended Achivements</h3>

<div id="ach_list">
<table class="displaytable">
<tr>
<td></td>
<td></td>
    <td>Achievement</td>
    <td>Points</td>
    <td width="200">Category</td>
    <td width="200"></td>
    
</tr>

<%
    
    int i = 1;
    foreach (var achievement in Model.RecommendedAchievements)
    {
        if (achievement == null)
        {
            continue;
        }
        %>
        <tr>
        <td><%= i %> (<%= achievement.Rank %>)</td>
        <td><img src="<%= ImageHelper.MediumImage(achievement) %>" border="0" /></td>
    <td><a href="http://www.wowhead.com/achievement=<%= achievement.BlizzardID %>"><%= achievement.Name %></a><br /><span class="achdescription"><%=achievement.Description %></span></td>
    <td><%= achievement.Points %></td>
    <td><%= CategoryHelper.CategoryName(achievement.ParentCategory == "-1" ? achievement.Category : achievement.ParentCategory)%></td>
    <td><%= CategoryHelper.CategoryName(achievement.ParentCategory == "-1" ? achievement.ParentCategory : achievement.Category)%></td>
    
</tr>

        <%
        i++;
    }

    %>

    </table>
    </div>
</asp:Content>

