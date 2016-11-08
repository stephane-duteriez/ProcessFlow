<%@ Register TagPrefix="uc" TagName="ProcessFlow" 
  Src="~/Areas/Lexigone/Controls/ProcessFlowControl.ascx" %>

<asp:Content ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" runat="server" media="screen" href="~/css/process_flow.css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <uc:ProcessFlow runat="server" entityname="lex_papertray" />
    <%: Html.HtmlAttribute("adx_copy", cssClass: "page-copy") %>
</asp:Content>
