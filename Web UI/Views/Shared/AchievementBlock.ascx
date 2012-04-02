<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PlayerAchievement>" %>
<%@ Import Namespace="Web_UI.Models" %>
<div class="recentachievement">

<%
    
    var achievement = Model.Achievement; %>
        <span class="recentTitle"><%= achievement.Name %> </span>
        <span class="recentPoints"><%= achievement.Points %></span>
        <span class="recentIcon"><img class="recentIconImage" src="<%= ImageHelper.LargeImage(achievement) %>" border=0 /></span>
        <span class="recentEarned"><%= Model.WhenAchieved.ToString("MM-dd-yyyy")%></span>
        <span class="recentDescription"><%= achievement.Description %></span>
        
        
        </div>

