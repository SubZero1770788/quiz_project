function addQuestion() {
  const container = document.getElementById("questions-container");

  // ✅ FIX: Get max value from all Index hidden fields
  const usedIndices = [
    ...container.querySelectorAll("input[name^='Questions'][name$='.Index']"),
  ]
    .map((input) => parseInt(input.value))
    .filter((n) => !isNaN(n));
  const bindingIndex =
    usedIndices.length > 0 ? Math.max(...usedIndices) + 1 : 0;

  const displayNumber = container.querySelectorAll(".question-box").length + 1;

  const questionDiv = document.createElement("div");
  questionDiv.className = "mb-4 border p-3 rounded bg-light question-box";
  questionDiv.id = `question-${bindingIndex}`;
  questionDiv.setAttribute("data-question-index", bindingIndex);

  questionDiv.innerHTML = `
    <button type="button" class="btn btn-danger mb-2" onclick="removeQuestion(${bindingIndex})">Remove</button>
    <h5>Question ${displayNumber}</h5>

    <input type="hidden" name="Questions[${bindingIndex}].QuestionId" value="0" />
    <input type="hidden" name="Questions[${bindingIndex}].Index" value="${bindingIndex}" />

    <div class="form-group">
      <label>Description</label>
      <input name="Questions[${bindingIndex}].Description" class="form-control" required />
    </div>
    <div class="form-group">
      <label>Score</label>
      <input type="number" name="Questions[${bindingIndex}].QuestionScore" class="form-control" value="1" required />
    </div>
    <div class="form-group">
      <label>How many answers?</label>
      <input type="number" min="1" max="10" class="form-control" placeholder="e.g. 4"
             onchange="addAnswers(this, ${bindingIndex})" />
    </div>
    <div id="answers-${bindingIndex}" class="mt-2"></div>
  `;

  container.appendChild(questionDiv);
}

//Add answers depending on ammount user wants to add
function addAnswers(input, qCount) {
  const count = parseInt(input.value);
  const answersContainer = document.getElementById(`answers-${qCount}`);

  answersContainer.innerHTML = "";

  for (let i = 0; i < count; i++) {
    const wrapper = document.createElement("div");
    wrapper.className = "form-group";

    //Actual answer
    wrapper.innerHTML = `
    <label>Answer ${i + 1}</label>
        <input name="Questions[${qCount}].Answers[${i}].Description" 
        class="form-control mb-1" required />
    <div class="form-check">
                <input class="form-check-input" type="checkbox" name="Questions[${qCount}].Answers[${i}].IsCorrect" value="true" id="correct-${qCount}-${i}">
            <label class="form-check-label" for="correct-${qCount}-${i}">
            Mark as correct
        </label>
    </div>`;
    answersContainer.appendChild(wrapper);
  }
}

function removeQuestion(questionIndex) {
  const container = document.getElementById("questions-container");
  const q = container.querySelector(`[data-question-index="${questionIndex}"]`);
  if (!q) return;

  // Remove the question block from DOM
  q.remove();

  // Re-number remaining questions
  const allQuestions = container.querySelectorAll("[data-question-index]");
  allQuestions.forEach((el, i) => {
    // Update visible title
    const title = el.querySelector("h5");
    if (title) title.textContent = `Question ${i + 1}`;

    // Optionally update input names too if you're binding on order (less safe)
    // But if you're binding using an Index or QuestionId, this is not needed.
  });
}
