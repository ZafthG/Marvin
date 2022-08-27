# Marvin
 Bot Marvin para o Dicord.
<br/>
<br/>
Por padrão o Token do Discord deve ser armazenado me uma variável ambiente com Key ```MARVIN_TOKEN```. Porém, é possível setar outro nome no arquivo 'bot.conf'. <br/>
Esse arquivo 'bot.conf' deve estar no diretório raiz do programa, caso contrário, teremos um erro
e uma falha de inicialização.<br/>
Segue exemplo para o arquivo 'bot.conf' que deve estar na pasta raiz do programa compilado:<br/>
<b>Usar UTF-8</b>.
<br/>
```
# --------------------------------------------------------------------- 
#		MARVIN CONF FILE
# ---------------------------------------------------------------------
#	Por ZafthG (Gabriel Ferreira)
# ---------------------------------------------------------------------

#   > Configurações do MySQL
DB_SERVER localhost
DB_USER root
DB_PASS @'null'
DB_NAME marvin_db

DB_CONN_TRIES 5

#   > Configurações de variáveis de ambiente
# O Token do discord deve ser armazenado em uma variável de ambiente
# e o nome dessa variável deve ser informado aqui.
E_V_MARVIN_TK MARVIN_TOKEN

#   > Configurações de comando
BOT_START_COMMAND .

# --------------------------------------------------------------------
#		Fim do arquivo de configurações
# --------------------------------------------------------------------
# @'null' -> indica um valor nulo, exemplo: string t = ""
```