<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.Ajuda>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Alterar
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">
        $(function() {
            //CKEditor
            $('textarea.editor').ckeditor(function() { /* callback code */ }, { 
			    //toolbar: [['Bold', 'Italic', 'Strike', '-','Undo','Redo', 'Link','Unlink','-', 'Image', 'Smiley', 'TextColor']],
                width: '98%',
			    height: 500,
			    resize_minWidth: '98%',
			    resize_maxWidth: '98%',
			    resize_minHeight: 300,
			    defaultLanguage: 'pt-br'
		    });
        });
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
    	<h4></h4>
        <h2>Ajuda - Alterar</h2>
        <h4></h4>
    </div>
    
    <div class="conteudo-form">
    
        <%=Html.ValidationSummary() %>
        
    	<form method="post" action="">
    	    <div class="divEditor">
    	        <%= Html.TextAreaFor(model => model.Texto, new { @class = "editor validate[required,length[0,5000]]" })%>
    	    </div>
    	    
    	    <ul style="float: left;">
                <li class="liSubmit">
                    <input class="submit" type="submit" value="Gravar" />
                </li>
            </ul>
    	    
        </form>
    </div>
    
</asp:Content>