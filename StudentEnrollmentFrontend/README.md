# Sistema de Alunos - Frontend

Um frontend feito com Vue.js 3 e Vuetify para gerenciar alunos.

## O que faz

- Cadastrar novos alunos com Nome, Email, RA e CPF
- Listar todos os alunos cadastrados
- Editar dados básicos (nome e email)
- Remover alunos quando necessário

## Tecnologias utilizadas
- Vue.js 3 - para construir interfaces de usuário interativas
- Vuetify 3 - para design de interfaces
- TypeScript - para tipagem estática e melhores práticas
- Vite - para build rápida e fácil
- Pinia - para gerenciar o estado da aplicação
- Vue Router - para navegação entre telas
- Axios - para fazer requisições HTTP

## Para rodar

Você vai precisar do Node.js 18 ou mais recente, recomendo o 20.

```bash
# instalar as coisas
npm install

# rodar em desenvolvimento
npm run dev
```

Basta acessar `http://localhost:3000`. Deixe a API rodando em `http://localhost:5078` para que tudo funcione.

## Como está organizado

```
src/
├── components/          # componentes reutilizáveis
├── views/              # telas principais
│   ├── StudentsListView.vue    # lista os alunos
│   └── StudentFormView.vue     # formulário para criar/editar
├── stores/             # onde fica o estado (Pinia)
├── services/           # para conexão com a API
├── types/              # tipos do TypeScript
└── router/             # configuração das rotas
```

## O que funciona

**Lista de alunos**: Uma tabela que mostra todos os alunos, com busca que funciona por qualquer campo. Tem botões para editar e excluir.

**Cadastro**: Formulário para adicionar aluno novo. Validando cpf, email, ra e nome. Campos obrigatórios e com validação de formato.

**Edição**: Carrega os dados do aluno e deixa editar nome e email. RA e CPF ficam bloqueados porque não podem mudar.

**Navegação**: Menu lateral.

## Validações que funcionam

- **Nome**: obrigatório, entre 2 e 100 caracteres
- **Email**: email único
- **RA**: obrigatório e único
- **CPF**: valida CPF brasileiro, máscara automática, único, não muda

## Como conversa com a API

Usa os endpoints: listar, buscar, criar, atualizar e excluir alunos. Se der erro, mostra uma mensagem para o usuário.

## Scripts

```bash
npm run dev      # desenvolvimento
npm run build    # build para produção
npm run preview  # ver como ficou a build
```

## Se algo der errado

- **Não carrega dados**: verificar se a API está rodando
- **Erro de CORS**: verificar configuração da API
- **Build quebra**: rodar `npm install` de novo

## Exemplos de uso da API
O frontend está configurado para fazer proxy das requisições `/api/*` direto para a API em `localhost:5078`, então deve funcionar se tudo estiver rodando certinho.

---