<div class="menu2">
	<ul>
		<% if (Sessao.Site.UsuarioLogado())
			{  %>
		<li>
			<%=Html.MenuLink("Home", "index", "home", ViewContext, new List<CustomRoute> { 
				new CustomRoute("index", "gerente"),
				new CustomRoute("index", "agencia"),
				new CustomRoute("index", "home")
			})%>
		</li>
		<%} %>
						
		<li>
			<%=Html.MenuLinkPermissao("Agências", "agencias", "gerente", ViewContext) %>
		</li>
				
		<li>
			<%=Html.MenuLinkPermissao("Produtos", "index", "produtos", ViewContext)%>
			<ul class="submenu">
								
			</ul>
		</li>
				
		<li>
			<%=Html.MenuLinkPermissao("Histórico", "historico", "agencia", ViewContext)%>
		</li>
				
		<li>
			<%=Html.MenuLinkPermissao("Gerenciador de Arquivos", "index", "gerenciador", ViewContext)%>
		</li>
				
		<li>
			<%=Html.MenuLinkPermissao("Agências", "index", "agencias", ViewContext) %>
		</li>
				
		<li>
			<%=Html.MenuLinkPermissao("Gerentes", "index", "gerentes", ViewContext) %>
		</li>
				
		<li>
			<%=Html.MenuLinkPermissao("Territórios", "index", "territorios", ViewContext) %>
		</li>
				
		<li>
			<%=Html.MenuLinkPermissao("Doutores", "index", "doutores", ViewContext) %>
		</li>
				
				
		<%if (!Sessao.Site.UsuarioTerritorioLogado() && Sessao.Site.UsuarioInfo.IsAdministrador())
          {  %>
				
				
		<li>
			<%=Html.MenuLinkPermissao("Relatórios", "index", "relatorios", ViewContext)%>
				
			<ul class="submenu">
				<li>
					<%=Html.MenuLinkPermissao("Produtos", "index", "relatorios")%>
				</li>
				<li>
					<%=Html.MenuLinkPermissao("Doutores", "doutores", "relatorios")%>
				</li>
				<li>
					<%=Html.MenuLinkPermissao("Download", "territorios", "relatorios")%>
				</li>
				<li>
					<%=Html.MenuLinkPermissao("Versão", "versao", "relatorios")%>
				</li>
			</ul>
		</li>
						
						
				
		<% }
			else { %>
				
			<%=Html.MenuLinkPermissao("Relatórios", "index", "relatorios", ViewContext)%>
				
		<%} %>
				
		<% if (Sessao.Site.VerificaPermissao("index", "paginas") || Sessao.Site.VerificaPermissao("index", "emails") || Sessao.Site.VerificaPermissao("index", "erros") || Sessao.Site.VerificaPermissao("index", "logs"))
			{  %>
				
		<li>
				
			<%=Html.MenuLink("Sistema", "index", "usuarios", ViewContext, 
				new List<CustomRoute> { 
					new CustomRoute("index","paginas"), 
					new CustomRoute("index","emails"),
					new CustomRoute("index","erros"),
					new CustomRoute("index","logs") 
									
				})%>
				
			<ul class="submenu">
				
				<% if (Sessao.Site.VerificaPermissao("index", "usuarios") || Sessao.Site.VerificaPermissao("index", "grupos"))
					{  %>
				
					<li>
						<%=Html.MenuLinkPermissao("Usuários", "index", "usuarios")%>
					</li>
					<li>
						<%=Html.MenuLinkPermissao("Grupos/Permissões", "index", "grupos")%>
					</li>
				
					<li>
						<%=Html.MenuLink("Importar", "index", "importacao", ViewContext)%>
					</li>
				<% } %>
				
				<li>
					<%=Html.MenuLinkPermissao("Grade", "grade", "produtos")%>
				</li>
				<li>
					<%=Html.MenuLinkPermissao("Ação/Controladores", "index", "paginas")%>
				</li>
				<li>
					<%=Html.MenuLinkPermissao("Emails", "index", "emails")%>
				</li>
				<li>
					<%=Html.MenuLinkPermissao("Erros", "index", "erros")%>
				</li>
				<li>
					<%=Html.MenuLinkPermissao("Logs", "index", "logs")%>
				</li>
			</ul>
							
				<li>
					<%=Html.MenuLinkPermissao("Dúvidas?", "index", "produtos", ViewContext)%>
				</li>            
		</li>
						
		<% } %>
	</ul>
</div>