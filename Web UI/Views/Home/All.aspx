<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CharacterList>" MasterPageFile="~/Views/Shared/ViewMasterPage1.Master" %>
<%@ Import Namespace="Web_UI.Models" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div>
   
   <% Html.RenderPartial("RankingView", Model); %>
        
    </div>
</asp:Content>
