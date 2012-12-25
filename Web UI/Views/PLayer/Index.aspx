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

     <script>
         $(document).ready(function () {
             $("#tabs").tabs().addClass('ui-tabs-vertical ui-helper-clearfix');
             $("#tabs li").removeClass('ui-corner-top').addClass('ui-corner-left');
         });
  </script>

    <style type="text/css">

    /* Vertical Tabs
----------------------------------*/
.ui-tabs-vertical { width: 58em; }
.ui-tabs-vertical .ui-tabs-nav { padding: .2em .1em .2em .2em; float: left; width: 15em; }
.ui-tabs-vertical .ui-tabs-nav li { clear: left; width: 100%; border-bottom-width: 1px !important; border-right-width: 0 !important; margin: 0 -1px .2em 0; }
.ui-tabs-vertical .ui-tabs-nav li a { display:block; }
.ui-tabs-vertical .ui-tabs-nav li.ui-tabs-selected { padding-bottom: 0; padding-right: .1em; border-right-width: 1px; border-right-width: 1px; }
.ui-tabs-vertical .ui-tabs-panel { padding: 1em; float: right; width: 40em;}
        </style>

<h3>Recommended Achivements</h3>

<div id="ach_list">

    <div id="tabs">
    <ul>
        <li><a href="#top5"><span>Top 5</span></a></li>
        <%
            foreach(int value in Enum.GetValues(typeof(AchievementSherpa.Business.AchievementCategories)))
            {
                string name = Enum.GetName(typeof(AchievementSherpa.Business.AchievementCategories), value);
                string displayName =  CategoryHelper.NiceEnumName((AchievementSherpa.Business.AchievementCategories)value);
                if ( Model.RecommendedAchievements.Where( a => a.CategoryID == value).Count() > 0 )
                {
                %>
                <li><a href="#<%= name %>"><span><%= displayName %> (<%=  Model.RecommendedAchievements.Where( a => a.CategoryID == value || a.ParentCategoryID == value).Count() %>)</span></a></li>       
                    <%
                    }
            }
            
         %>
    </ul>
    <div id="top5">
        <% Html.RenderPartial("_PartialPage1", Model.RecommendedAchievements.Take(5)); %>
    </div>
         <%
            foreach(int value in Enum.GetValues(typeof(AchievementSherpa.Business.AchievementCategories)))
            {
                string name = Enum.GetName(typeof(AchievementSherpa.Business.AchievementCategories), value);
                string displayName = CategoryHelper.NiceEnumName((AchievementSherpa.Business.AchievementCategories)value);
                if ( Model.RecommendedAchievements.Where( a => a.CategoryID == value).Count() > 0 )
                {
                %>
            <div id="<%= name %>">
        <% Html.RenderPartial("_PartialPage1", Model.RecommendedAchievements.Where( a => a.CategoryID == value || a.ParentCategoryID == value)); %>
    </div>
                
                    <%
                    }
            }
            
         %>
</div>

    </div>
</asp:Content>

