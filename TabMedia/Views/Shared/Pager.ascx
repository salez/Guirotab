<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<String>" %>

<div class="pager">

    <img src="<%=Url.Content("~/") %>Scripts/pagerimages/first.png" class="first">
    <img src="<%=Url.Content("~/") %>Scripts/pagerimages/prev.png" class="prev">
    <input type="text" disabled="disabled" class="pagedisplay">
    <img src="<%=Url.Content("~/") %>Scripts/pagerimages/next.png" class="next">
    <img src="<%=Url.Content("~/") %>Scripts/pagerimages/last.png" class="last">
    
    <label>Exibir:</label>
    
    <select class="pagesize">
        <option value="10">10 por página</option>
        <option value="20">20 por página</option>
        <option value="30" selected="selected">30 por página</option>
        <option value="40">40 por página</option>
        <option value="50">50 por página</option>
    </select>
    
    <label>Ir para página:</label><input type="text" class="gotopage" />
    <label><%=Model %></label>
</div>