<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CharacterList>" %>
<%@ Import Namespace="Web_UI.Models" %>
 <table class="results">
    <tr>
    <td>Rank</td>
            <td>Class</td>
                <td>Name</td>
                <td>Race</td>
                
                <td>Server</td>
            <td>Guild</td>    
                <td>Points</td>
                <td>Cluster</td>
            </tr>
    <%
        int i = 1;
        foreach (AchievementSherpa.Business.Character character in Model.Characters)
        {
            
            string guildName = character.Guild;
            if ( string.IsNullOrEmpty(guildName))
            {
                guildName = " ";
            }
            %>
            <tr class="side_<%= character.Side %>">
            <td><%= i++ %></td>
            <td><img src="<%= ImageHelper.SmallClassImage(character) %>" border="0" /></td>
            <td class="name">
            <%= Html.ActionLink(character.Name,
                            "Index", "Player", new { region = character.Region, server = character.Server, player=character.Name }, null)%>
            </td>
            <td><img src="<%= ImageHelper.SmallRaceImage(character) %>" border="0" /></td>
                
            <td> <%= Html.ActionLink(character.Server,
                            "ServerRanking", "Ranking", new { server = character.Server, region = character.Region }, null)%></td>
                
                <td>
                <%= Html.ActionLink(guildName,
                "Guild", "Ranking", new {guildName=character.Guild, server=character.Server, region=character.Region}, null)%></td>
                
                <td class="points"><%= character.CurrentPoints %></td>
                <td class="points"><%= character.ClusterNumber %></td>
            </tr>
            <%
        }
        
         %></table>

 

