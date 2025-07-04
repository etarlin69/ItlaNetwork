﻿document.addEventListener('DOMContentLoaded', function () {

    
    function scrollToBottom(element) {
        if (element) {
            element.scrollTop = element.scrollHeight;
        }
    }

    
    const commentLists = document.querySelectorAll('.comment-list');
    commentLists.forEach(list => {
        scrollToBottom(list);
    });

    
    const mainContentArea = document.querySelector('.main-content-area');

    if (mainContentArea) {
        
        mainContentArea.addEventListener('click', function (event) {

            
            const toggleButton = event.target.closest('.toggle-comment-form');
            if (toggleButton) {
                event.preventDefault();
                const postCard = toggleButton.closest('.glass-card');
                const commentFormContainer = postCard.querySelector('.comment-form-container');
                if (commentFormContainer) {
                    commentFormContainer.classList.toggle('active');
                }
            }

            
            const reactionButton = event.target.closest('.reaction-btn');
            if (reactionButton) {
                event.preventDefault();

                const postCard = reactionButton.closest('.glass-card');
                const postId = reactionButton.dataset.postId;
                const reactionType = reactionButton.dataset.reactionType;

                fetch('/Reaction/ToggleReaction', {
                    method: 'POST',
                    body: new URLSearchParams({
                        'PostId': postId,
                        'ReactionType': reactionType
                    }),
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                })
                    .then(response => {
                        if (!response.ok) throw new Error('Error en la respuesta del servidor');
                        return response.json();
                    })
                    .then(data => {
                        const likeBtn = postCard.querySelector('.reaction-btn.like');
                        const dislikeBtn = postCard.querySelector('.reaction-btn.dislike');

                        
                        likeBtn.querySelector('.like-count').textContent = data.likeCount;
                        dislikeBtn.querySelector('.dislike-count').textContent = data.dislikeCount;

                        
                        likeBtn.classList.remove('active');
                        dislikeBtn.classList.remove('active');

                        if (data.currentUserReaction === 'Like') {
                            likeBtn.classList.add('active');
                        } else if (data.currentUserReaction === 'Dislike') {
                            dislikeBtn.classList.add('active');
                        }
                    })
                    .catch(error => console.error('Error al procesar la reacción:', error));
            }
        });

        
        mainContentArea.addEventListener('submit', function (event) {
            const form = event.target.closest('.new-comment-form');
            if (form) {
                event.preventDefault();

                const formData = new FormData(form);
                const actionUrl = form.getAttribute('action');
                const postId = formData.get('PostId');
                const content = formData.get('Content');
                const textarea = form.querySelector('textarea');

                
                const requestVerificationToken = form.querySelector('input[name="__RequestVerificationToken"]').value;

                if (!content || !content.trim()) return;

                fetch(actionUrl, {
                    method: 'POST',
                    body: formData,
                    headers: {
                        
                        'RequestVerificationToken': requestVerificationToken,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                })
                    .then(response => {
                        if (!response.ok) {
                            return response.text().then(text => { throw new Error(`Error del servidor: ${response.status} - ${text}`) });
                        }
                        return response.text();
                    })
                    .then(newCommentHtml => {
                        const commentListContainer = document.getElementById(`comment-list-${postId}`);
                        if (commentListContainer) {
                            const noCommentsMessage = commentListContainer.querySelector('.no-comments-message');
                            if (noCommentsMessage) noCommentsMessage.remove();

                            commentListContainer.insertAdjacentHTML('beforeend', newCommentHtml);
                            textarea.value = '';
                            scrollToBottom(commentListContainer);
                        }
                    })
                    .catch(error => console.error('Hubo un error al enviar el comentario:', error));
            }
        });
    }
});