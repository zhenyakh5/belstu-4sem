from PIL import Image, ImageTk, ImageEnhance, ImageFilter, ImageOps
import tkinter as tk
from tkinter import filedialog, messagebox

class ImageEditor:
    def __init__(self, root):
        self.root = root
        self.root.title("Image Editor")
        self.image = None
        self.original_image = None
        self.image_tk = None
        self.filename = None

        self.create_widgets()

    def create_widgets(self):
        self.canvas = tk.Canvas(self.root, width=600, height=400, bg="white")
        self.canvas.grid(row=0, column=0, columnspan=2)

        self.button_frame = tk.Frame(self.root)
        self.button_frame.grid(row=1, column=0, sticky="n")

        self.open_button = tk.Button(self.button_frame, text="Open", command=self.open_image)
        self.open_button.pack(side=tk.LEFT)

        self.reset_button = tk.Button(self.button_frame, text="Reset", command=self.reset_image)
        self.reset_button.pack(side=tk.LEFT)

        self.save_button = tk.Button(self.button_frame, text="Save", command=self.save_image)
        self.save_button.pack(side=tk.LEFT)

        self.edit_frame = tk.LabelFrame(self.root, text="Edit Options", padx=5, pady=5)
        self.edit_frame.grid(row=1, column=1, sticky="n")

        self.rotate_button = tk.Button(self.edit_frame, text="Rotate", command=self.rotate_image)
        self.rotate_button.pack(anchor="w")

        self.gray_button = tk.Button(self.edit_frame, text="Gray", command=self.convert_to_gray)
        self.gray_button.pack(anchor="w")

        self.light_button = tk.Button(self.edit_frame, text="Light", command=self.adjust_light)
        self.light_button.pack(anchor="w")

        self.contrast_button = tk.Button(self.edit_frame, text="Contrast", command=self.adjust_contrast)
        self.contrast_button.pack(anchor="w")

        self.blur_label = tk.Label(self.edit_frame, text="Blur Amount")
        self.blur_label.pack(anchor="w")

        self.blur_scale = tk.Scale(self.edit_frame, from_=0, to_=20, orient=tk.HORIZONTAL, command=self.apply_blur)
        self.blur_scale.set(0)
        self.blur_scale.pack(anchor="w")

        self.sharpen_button = tk.Button(self.edit_frame, text="Sharpen", command=self.apply_sharpen)
        self.sharpen_button.pack(anchor="w")

    def open_image(self):
        self.filename = filedialog.askopenfilename(filetypes=[("Image files", "*.png;*.jpg;*.jpeg;*.bmp;*.gif")])
        if self.filename:
            self.original_image = Image.open(self.filename)
            self.image = self.original_image.copy()
            self.display_image()

    def reset_image(self):
        if self.original_image:
            self.image = self.original_image.copy()
            self.display_image()

    def save_image(self):
        if self.image:
            save_path = filedialog.asksaveasfilename(defaultextension=".png", filetypes=[("PNG files", "*.png")])
            if save_path:
                self.image.save(save_path)
                messagebox.showinfo("Success", "Image saved successfully!")

    def display_image(self):
        self.image_tk = ImageTk.PhotoImage(self.image)
        self.canvas.create_image(0, 0, anchor=tk.NW, image=self.image_tk)
        self.canvas.config(width=self.image.width, height=self.image.height)

    def rotate_image(self):
        if self.image:
            self.image = self.image.rotate(-90, expand=True)
            self.display_image()

    def convert_to_gray(self):
        if self.image:
            self.image = ImageOps.grayscale(self.image)
            self.display_image()

    def adjust_light(self):
        if self.image:
            enhancer = ImageEnhance.Brightness(self.image)
            self.image = enhancer.enhance(1.5)
            self.display_image()

    def adjust_contrast(self):
        if self.image:
            enhancer = ImageEnhance.Contrast(self.image)
            self.image = enhancer.enhance(1.5)
            self.display_image()

    def apply_blur(self, value):
        if self.original_image:
            blur_value = int(value)
            self.image = self.original_image.copy()
            self.image = self.image.filter(ImageFilter.GaussianBlur(radius=blur_value))
            self.display_image()

    def apply_sharpen(self):
        if self.image:
            self.image = self.image.filter(ImageFilter.SHARPEN)
            self.display_image()

if __name__ == "__main__":
    root = tk.Tk()
    app = ImageEditor(root)
    root.mainloop()
