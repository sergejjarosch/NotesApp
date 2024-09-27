document.addEventListener('DOMContentLoaded', function () {
    // Finde alle Elemente mit der Klasse "quill-editor"
    var editors = document.querySelectorAll('.quill-editor');

    editors.forEach(function (editor, index) {
        // Initialisiere Quill für jedes Editor-Element
        var quill = new Quill(editor, {
            theme: 'snow'
        });

        // Verknüpfe das zugehörige hidden input-Feld (welches das Model speichert)
        var hiddenInput = editor.closest('.note-text-field').querySelector('input[type="hidden"]');

        // Setze den initialen Inhalt des Editors, falls das hidden input einen Wert enthält
        if (hiddenInput.value) {
            quill.clipboard.dangerouslyPasteHTML(hiddenInput.value);
        }

        // Speichern des Quill-Inhalts im versteckten input-Feld bei Formularabsendung
        editor.closest('form').onsubmit = function () {
            var content = quill.root.innerHTML;
            hiddenInput.value = content; // Setze den HTML-Inhalt in das hidden input
        };
    });
});
