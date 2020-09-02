export const checkImageExist = (path) => {
  return new Promise((resolve) => {
    const img = new Image();
    img.src = path;
    img.onload = () => resolve(true);
    img.onerror = () => resolve(false);
  });
};
