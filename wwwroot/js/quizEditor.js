function addQuestion(questionCounter) {
  const container = document.getElementById("questions-container");

  const questionDiv = document.createElement("div");
  questionDiv.className = "mb-4 border p-3 rounded bg-light";
  questionDiv.setAttribute("data-question-index", questionCounter);

  //actual question
  questionDiv.innerHTML = `
                              <button class="btn btn-danger" onclick="removeQuestion(@i)">Remove</button>
                                <h5>Question ${questionCounter + 1}</h5>

                            <div class="form-group">
                                <label>Description</label>
                                    <input name="Questions[${questionCounter}].Description" 
                                    class="form-control" required />
                            </div>
                            <div class="form-group">
                                <label>Score</label>
                                    <input type="number" name="Questions[${questionCounter}].QuestionScore" 
                                    class="form-control" value="1" required />
                            </div>
                            <div class="form-group">
                                <label>How many answers?</label>
                                <input type="number" min="1" max="10" class="form-control" placeholder="e.g. 4"
                                        onchange="addAnswers(this, ${questionCounter})" />
                            </div>
                                <div id="answers-${questionCounter}" class="mt-2"></div>
                        `;

  container.appendChild(questionDiv);
  questionCounter++;
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
                                </div>
                            `;
    answersContainer.appendChild(wrapper);
  }
}

function removeQuestion(question) {
  const q = document.querySelector(`#question-${question}`);
  q.remove();

  const questionElements = document.querySelectorAll('[id^="question-"]');
  questionElements.forEach((el, newIndex) => {
    el.id = `question-${newIndex}`;
    el.querySelectorAll("input, textarea, select").forEach((input) => {
      if (input.name) {
        input.name = input.name.replace(/\[\d+\]/g, `[${newIndex}]`);
      }
    });

    const answerInput = el.querySelector("input[type='number'][onchange]");
    if (answerInput) {
      answerInput.setAttribute("onchange", `addAnswers(this, ${newIndex})`);
    }

    const removeBtn = el.querySelector("button[onclick*='removeQuestion']");
    if (removeBtn) {
      removeBtn.setAttribute("onclick", `removeQuestion(${newIndex})`);
    }

    const title = el.querySelector("h5");
    if (title) title.textContent = `Question ${newIndex + 1}`;
  });

  questionCounter = document.querySelectorAll('[id^="question-"]').length;
}
