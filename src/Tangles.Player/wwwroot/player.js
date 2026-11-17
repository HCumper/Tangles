// Browser adapters only: geometry, pointer/keyboard input, local files and storage.
// Puzzle rules and every accepted move live in the shared F# library.
window.tangles = (() => {
  let reference;
  let attachedBoard;
  let attachedFile;
  let puzzleKey;
  let last;
  let cursor = { x: 0, y: 0 };
  let drawing = false;
  let keyboardDrawing = false;
  let queue = Promise.resolve();

  const send = (action, x = 0, y = 0) => reference.invokeMethodAsync("BoardInput", action, x, y);
  const enqueue = action => {
    queue = queue.then(action).catch(error => {
      console.error("Board interaction failed", error);
      drawing = keyboardDrawing = false;
      return reference.invokeMethodAsync("FileError", "The interaction could not be completed. Try Undo or reload the player.");
    });
  };
  const size = board => ({ width: Number(board.dataset.width), height: Number(board.dataset.height) });
  const locate = (board, event) => {
    const rect = board.getBoundingClientRect();
    const dimensions = size(board);
    const x = Math.floor((event.clientX - rect.left) / rect.width * dimensions.width);
    const y = Math.floor((event.clientY - rect.top) / rect.height * dimensions.height);
    return x >= 0 && y >= 0 && x < dimensions.width && y < dimensions.height ? { x, y } : null;
  };
  const showCursor = board => {
    const marker = board.querySelector("#keyboard-cursor");
    if (!marker) return;
    marker.setAttribute("x", cursor.x * 64 + 4);
    marker.setAttribute("y", cursor.y * 64 + 4);
    marker.style.visibility = "visible";
  };
  const trace = async target => {
    if (!last) return;
    // Fast pointer movements can skip cells. Emit individual orthogonal steps,
    // stopping at the first move that the F# rules reject.
    while (last.x !== target.x || last.y !== target.y) {
      const dx = target.x - last.x, dy = target.y - last.y;
      const next = Math.abs(dx) >= Math.abs(dy)
        ? { x: last.x + Math.sign(dx), y: last.y }
        : { x: last.x, y: last.y + Math.sign(dy) };
      if (!await send("move", next.x, next.y)) break;
      last = next;
    }
  };

  function attach(dotnetReference) {
    reference = dotnetReference;
    const board = document.getElementById("puzzle-board");
    if (board && board.dataset.puzzle !== puzzleKey) {
      puzzleKey = board.dataset.puzzle;
      cursor = { x: 0, y: 0 };
      last = null;
      drawing = keyboardDrawing = false;
    }
    if (board && board !== attachedBoard) {
      attachedBoard = board;
      board.addEventListener("pointerdown", event => {
        if (event.button !== 0 || !event.isPrimary) return;
        const cell = locate(board, event);
        if (!cell) return;
        event.preventDefault();
        board.focus({ preventScroll: true });
        board.setPointerCapture(event.pointerId);
        drawing = true;
        keyboardDrawing = false;
        const marker = board.querySelector("#keyboard-cursor");
        if (marker) marker.style.visibility = "hidden";
        enqueue(async () => { last = await send("begin", cell.x, cell.y) ? cell : null; });
      });
      board.addEventListener("pointermove", event => {
        if (!drawing || !event.isPrimary) return;
        const target = locate(board, event);
        if (target) enqueue(() => trace(target));
      });
      const finish = event => {
        if (!drawing) return;
        const target = locate(board, event);
        drawing = false;
        if (board.hasPointerCapture(event.pointerId)) board.releasePointerCapture(event.pointerId);
        enqueue(async () => {
          if (target) await trace(target);
          await send("end");
          last = null;
        });
      };
      board.addEventListener("pointerup", finish);
      board.addEventListener("pointercancel", event => {
        drawing = false;
        enqueue(async () => { await send("end"); last = null; });
      });
      board.addEventListener("keydown", event => {
        const delta = { ArrowLeft: [-1, 0], ArrowRight: [1, 0], ArrowUp: [0, -1], ArrowDown: [0, 1] }[event.key];
        if (delta) {
          event.preventDefault();
          const dimensions = size(board);
          cursor = {
            x: Math.max(0, Math.min(dimensions.width - 1, cursor.x + delta[0])),
            y: Math.max(0, Math.min(dimensions.height - 1, cursor.y + delta[1]))
          };
          showCursor(board);
          const target = { ...cursor };
          if (keyboardDrawing) enqueue(() => trace(target));
        } else if (event.key === " " || event.key === "Enter") {
          event.preventDefault();
          if (event.repeat) return;
          showCursor(board);
          const target = { ...cursor };
          const wasDrawing = keyboardDrawing;
          keyboardDrawing = !keyboardDrawing;
          enqueue(async () => {
            if (wasDrawing) { await send("end"); last = null; }
            else { const started = await send("begin", target.x, target.y); last = started ? target : null; if (!started) keyboardDrawing = false; }
          });
        } else if (event.key === "Escape" || ((event.ctrlKey || event.metaKey) && event.key.toLowerCase() === "z")) {
          event.preventDefault();
          keyboardDrawing = false;
          last = null;
          enqueue(() => send(event.key === "Escape" ? "end" : "undo"));
        }
      });
      board.addEventListener("blur", () => {
        if (keyboardDrawing) { keyboardDrawing = false; enqueue(() => send("end")); }
      });
    }
    const file = document.getElementById("pack-file");
    if (file && file !== attachedFile) {
      attachedFile = file;
      file.addEventListener("change", async () => {
        const selected = file.files[0];
        if (!selected) return;
        try {
          if (selected.size > 5 * 1024 * 1024) throw new Error("Please choose a JSON puzzle pack smaller than 5 MB.");
          await reference.invokeMethodAsync("ImportPack", await selected.text(), selected.name);
        } catch (error) {
          await reference.invokeMethodAsync("FileError", error.message || "The file could not be read.");
        } finally { file.value = ""; }
      });
    }
  }
  return {
    attach,
    load: key => { try { return localStorage.getItem(key); } catch { return null; } },
    save: (key, json) => { try { localStorage.setItem(key, json); return true; } catch { return false; } }
  };
})();
