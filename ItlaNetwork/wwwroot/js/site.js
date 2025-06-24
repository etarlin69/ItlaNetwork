// Espera a que todo el contenido del DOM esté cargado.
document.addEventListener('DOMContentLoaded', function () {

    // --- Lógica para el scroll, comentarios y reacciones (sin cambios) ---
    function scrollToBottom(element) {
        if (element) {
            element.scrollTop = element.scrollHeight;
        }
    }
    const commentLists = document.querySelectorAll('.comment-list');
    commentLists.forEach(list => { scrollToBottom(list); });
    const commentToggleButtons = document.querySelectorAll('.toggle-comment-form');
    commentToggleButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault();
            const postCard = this.closest('.glass-card');
            const commentFormContainer = postCard.querySelector('.comment-form-container');
            if (commentFormContainer) {
                commentFormContainer.classList.toggle('active');
            }
        });
    });
    const newCommentForms = document.querySelectorAll('.new-comment-form');
    newCommentForms.forEach(form => {
        form.addEventListener('submit', function (event) {
            event.preventDefault();
            const formData = new FormData(this);
            const actionUrl = this.getAttribute('action');
            const postId = formData.get('PostId');
            const content = formData.get('Content');
            const textarea = this.querySelector('textarea');
            if (!content.trim()) { return; }
            fetch(actionUrl, { method: 'POST', body: formData, headers: { 'X-Requested-With': 'XMLHttpRequest' } })
                .then(response => { if (!response.ok) { throw new Error(`Error del servidor: ${response.status}`); } return response.text(); })
                .then(newCommentHtml => {
                    const commentListContainer = document.getElementById(`comment-list-${postId}`);
                    if (commentListContainer) {
                        const noCommentsMessage = commentListContainer.querySelector('.no-comments-message');
                        if (noCommentsMessage) { noCommentsMessage.remove(); }
                        commentListContainer.insertAdjacentHTML('beforeend', newCommentHtml);
                        textarea.value = '';
                        scrollToBottom(commentListContainer);
                    }
                })
                .catch(error => console.error('Hubo un error al enviar el comentario:', error));
        });
    });
    const reactionButtons = document.querySelectorAll('.reaction-btn');
    reactionButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault();
            const postId = this.dataset.postId;
            const reactionTypeString = this.dataset.reactionType;
            const reactionTypeValue = reactionTypeString === 'Like' ? 1 : 2;
            const postCard = this.closest('.glass-card');
            fetch('/Reaction/ToggleReaction', { method: 'POST', body: new URLSearchParams({ 'PostId': postId, 'ReactionType': reactionTypeValue }), headers: { 'Content-Type': 'application/x-www-form-urlencoded', 'X-Requested-With': 'XMLHttpRequest' } })
                .then(response => { if (!response.ok) throw new Error('Error en la respuesta'); return response.json(); })
                .then(data => {
                    postCard.querySelector('.like-count').textContent = data.likeCount;
                    postCard.querySelector('.dislike-count').textContent = data.dislikeCount;
                    const likeBtn = postCard.querySelector('.reaction-btn.like');
                    const dislikeBtn = postCard.querySelector('.reaction-btn.dislike');
                    likeBtn.classList.remove('active');
                    dislikeBtn.classList.remove('active');
                    if (data.currentUserReaction === 'Like') {
                        likeBtn.classList.add('active');
                    } else if (data.currentUserReaction === 'Dislike') {
                        dislikeBtn.classList.add('active');
                    }
                })
                .catch(error => console.error('Error al procesar la reacción:', error));
        });
    });


    
});