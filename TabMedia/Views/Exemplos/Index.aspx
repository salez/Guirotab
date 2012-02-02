<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Exemplos
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">
        //Paginacao JQuery

        var inicio;
        var ate;
        var qtdePag;
        var qtdePorPag = 20;
        var classeObjetoGeral = '.itens';
        var classeObjeto = '.item';

        function proximaPagina() {
            $(classeObjetoGeral + ' ' + classeObjeto).hide();

            for (var i = inicio; i < ate; i++) {
                $(classeObjetoGeral + ' ' + classeObjeto+':eq(' + i + ')').show();
            }

            $(classeObjetoGeral).fadeIn("slow");

            //$('.comentarios .comentario:eq('+pagina+')').fadeIn("slow");
            return false;
        }

        function pageselectCallback(page_index, jq) {

            inicio = page_index * qtdePorPag;
            ate = page_index * qtdePorPag + qtdePorPag;

            $("#numPaginas").text('página ' + (page_index + 1) + ' de ' + qtdePag);

            $(classeObjetoGeral).fadeOut("slow", proximaPagina);

            // se chegar no final esconde o "proximo"
            if (page_index == qtdePag - 1) {
                $('#Pagination .current').attr("style", "visibility: hidden");
            }

            //se for a primeira pagina esconde o "anterior"
            if (page_index == 0) {
                $('#Pagination .current').attr("style", "visibility: hidden");
            }

            return false;
        }

        /** 
        * Callback function for the AJAX content loader.
        */
        function initPagination() {
            var num_entries = $(classeObjetoGeral + ' ' + classeObjeto).length;
            // Create pagination element
            $("#Pagination").pagination(num_entries, {
                num_edge_entries: 10,
                num_display_entries: 10,
                callback: pageselectCallback,
                items_per_page: qtdePorPag,
                prev_class: "anterior",
                next_class: "proximo"
            });
        }

        $(document).ready(function() {
            qtdePag = Math.ceil($(classeObjetoGeral + ' ' + classeObjeto).length / qtdePorPag);
            $(classeObjetoGeral + ' ' + classeObjeto).hide();
            initPagination();

        });
        
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Formulario</h2>
    
    <br /><br />
    <form>
        <br />Required:<br /> <input id="required" class="validate[required]" type="text" />
        <br />
        <br />Length:<br /> <input id="length" class="validate[length[0,1]]" type="text" />
        <br />
        <br />Só Numero<br /> <input id="onlyNumber" class="validate[custom[onlyNumber]]" type="text" />
        <br />
        <br />Email<br /> <input id="email" class="validate[custom[email]]" type="text" />
        <br /><br />
        <input type="submit" />
    </form>
    
    <br />
    
    <h2>Conteúdo</h2>
    
    <fieldset>
        <% Html.RenderConteudoContexto("fale conosco"); %>
    </fieldset>
    
    <h2>Paginação</h2>
    
    <div id="numPaginas" style=""></div>
    <div style="" id="Pagination"></div>
    
    <ul class="itens">
        <% for (int i = 1; i <= 100; i++)
           { %>
        
        <li class="item">
            Item <%=i %>
        </li>
        
        <%} %>
    </ul>
    
</asp:Content>