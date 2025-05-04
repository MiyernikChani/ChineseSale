import { AbstractControl, ValidationErrors } from '@angular/forms';

// פונקציית ולידציה מותאמת אישית
export function futureDateValidator(control: AbstractControl): ValidationErrors | null {
  const value = control.value;

  // בדיקת פורמט בסיסי של תאריך
  const isValidDate = !isNaN(Date.parse(value));
  if (!isValidDate) {
    return { invalidDate: true }; // לא תאריך חוקי
  }

  // יצירת תאריך מהערך שהוזן
  const enteredDate = new Date(value);
  const enteredYear = enteredDate.getFullYear();

  // בדיקה אם השנה גדולה מ-6000
  if (enteredYear > 6000) {
    return { yearTooLarge: true }; // השנה גדולה מדי
  }

  // בדיקה אם התאריך עבר כבר
  const today = new Date();
  today.setHours(0, 0, 0, 0); // השווה עד רמת היום בלבד

  if (enteredDate < today) {
    return { pastDate: true }; // תאריך עבר
  }

  return null; // התאריך תקין
}
